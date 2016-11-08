using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml;
using Elasticsearch.Net;
using ESService.Enums;
using ESService.Interfaces;
using ESService.Models;
using ESService.Models.ModelsES;
using Nest;

namespace ES_WebApi.Services
{
    public class ESearchService : ISearchService
    {
        // todo - check path to your ES_Server
        private readonly string mainPath = @"http://localhost:9200/";

        private readonly ElasticClient _client;

        public ESearchService()
        {
            var nodes = new Uri[] {
                new Uri(mainPath)
            };

            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool).DisableDirectStreaming();

            _client = new ElasticClient(settings);
        }

        public ResultSearch GetSearch(FilterModel filter)
        {
            List<string> resultPaths = new List<string>();

            resultPaths = SendSearchRequest(filter);

            return new ResultSearch(resultPaths);
        }

        private List<string> SendSearchRequest(FilterModel sc)
        {
            //bool
            QueryContainer boolQuery = new BoolQuery() { };
            //must
            List<QueryContainer> mustquerys = new List<QueryContainer>();
            //should
            List<QueryContainer> shouldquerys = new List<QueryContainer>();

            int lastCommand = 0;
            // create Query
            foreach (var searchModel in sc.SearchModels)
            {
                QueryContainer qc = new QueryContainer();

                if (searchModel.Field.ToLower() == "any")
                {
                    searchModel.Field = "_all";
                }


                // if Contains => search Text
                if (searchModel.Oper == (int)Oper.Contains)
                {
                    qc = new TermQuery() { Field = searchModel.Field, Value = searchModel.Text.ToLower() };
                }

                //  if Less,Equal,Greater => search Number
                if (Enum.IsDefined(typeof(Oper), searchModel.Oper) && searchModel.Oper != (int)Oper.Contains)
                {
                    double d = 0;
                    Double.TryParse(searchModel.Text, out d);

                    if (searchModel.Oper == (int)Oper.Equal)
                    {
                        qc = new NumericRangeQuery() { Field = searchModel.Field, GreaterThanOrEqualTo = d, LessThanOrEqualTo = d };
                    }
                    if (searchModel.Oper == (int)Oper.Less)
                    {
                        qc = new NumericRangeQuery() { Field = searchModel.Field, LessThan = d };
                    }
                    if (searchModel.Oper == (int)Oper.Greater)
                    {
                        qc = new NumericRangeQuery() { Field = searchModel.Field, GreaterThan = d };
                    }
                }

                if (lastCommand == 0 && searchModel.NextOper == (int)OperForNext.And)
                {
                    mustquerys.Add(qc);
                }

                if (lastCommand == 0 && searchModel.NextOper == (int)OperForNext.Or)
                {
                    shouldquerys.Add(qc);
                }

                // if And => Must
                if (lastCommand == (int)OperForNext.And)
                    mustquerys.Add(qc);

                // if Or => Should
                if (lastCommand == (int)OperForNext.Or)
                    shouldquerys.Add(qc);

                lastCommand = searchModel.NextOper;
            }


            // ALL quries
            boolQuery = new BoolQuery() { Must = mustquerys, Should = shouldquerys };

            var r = new SearchRequest()
            {
                Size = 100,
                //MinScore = 0.5,
                Query = boolQuery
            };

            // send request
            var resultPaths = GetPaths(sc.Category, r);

            return resultPaths;
        }
        
        private List<string> GetPaths(string typeName, SearchRequest r)
        {
            List<string> resultPaths = new List<string>();
            try
            {
                if (typeName == "test_data")
                {
                    var res = _client.Search<SomeDataES>(r);
                    Console.WriteLine(res.Hits.Count());
                    foreach (var hit in res.Hits)
                    {
                        var result = hit.Source;

                        Console.WriteLine(result.PathTo);
                        resultPaths.Add(result.PathTo);
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("-----Error GetPaths" + ex.Message);
            }

            return resultPaths;
        }


        public ResultBase PostData(XmlDocument doc, string path)
        {
            try
            {

                object sendObject = null;
                string typeName = doc.DocumentElement?.LocalName;
                string amazonPath = path + " - " + typeName;

                // Parse to model
                sendObject = ParseToModel(doc, typeName, amazonPath);

                if (sendObject == null)
                {
                    return new ResultBase(false, "Category not found");
                }

                // send to ES
                var response = _client.IndexAsync(sendObject, idx => idx.Index(typeName)).Result;

                // get result message
                string resultString = $"{response.Index} Created = {response.Created} {response.ServerError}";
                var r = new ResultBase(response.Created, resultString);

                return r;
            }
            catch (Exception ex)
            {
                return new ResultBase(false, ex.Message);
            }

        }

        private static object ParseToModel(XmlDocument doc, string typeName, string path)
        {
            object sendObject = null;
            try
            {
                if (typeName == "test_data")
                {
                    sendObject = new SomeDataES(path, doc);
                }
            }
            catch (Exception ex)
            {
                return sendObject;
            }
            return sendObject;
        }
    }
}