using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJH.BaseTools.BasicExtend;
using ZJH.BaseTools.Net;

namespace ZJH.EsriGIS.Server
{
    public class MapServerLayer
    {
        public string url;//服务地址（不包括最后的数字）
        public int id;//图层id
        public string name;//图层名称
        public int parentid;//父节点id
        public MapServerLayer(string url, JObject obj)
        {
            this.url = url;
            id = obj.Value<int>("id");
            name = obj.Value<string>("name");
            parentid = obj.Value<int>("parentLayerId");
        }
        public MapServerLayer(string url, int id, string name, int parentid) {
            this.url = url;
            this.id = id;
            this.name = name;
            this.parentid = parentid;
        }

        /// <summary>
        /// 通过MapServer获取到的图层信息
        /// </summary>
        JObject _LayerInfo = null;
        JObject LayerInfo {
            get {
                if (true) {
                    string result = ZWebClient.DownloadString($"{url}/{id}?f=pjson");
                    _LayerInfo = JObject.Parse(result);
                }
                return _LayerInfo;
            }
        }

        /// <summary>
        /// 图层的OBJECTID字段名
        /// </summary>
        string _ObjectIdFieldName = "";
        public string ObjectIdFieldName {
            get {
                if (_ObjectIdFieldName.IsNullOrWhiteSpace()) {
                    _ObjectIdFieldName = "OBJECTID";
                    JArray fields = LayerInfo.Value<JArray>("fields");
                    foreach (JObject field in fields) {
                        string type = field.Value<string>("type");
                        if (type == "esriFieldTypeOID") {
                            _ObjectIdFieldName = field.Value<string>("name");
                            break;
                        }
                    }
                }
                return _ObjectIdFieldName;
            }
        }

        /// <summary>
        /// 获取Feature的总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetFeautreNum(string where = "1=1")
        {
            string queryStr = $"query?where={where}&time=&returnCountOnly=true&returnIdsOnly=true&returnGeometry=false&outFields=*&f=pjson";
            string result = ZWebClient.DownloadString($"{url}/{id}/{queryStr}");
            JObject obj = JObject.Parse(result);
            JToken token = null;
            if (obj.TryGetValue("objectIds", out token))
            {
                return ((JArray)token).Count();
            }
            else if (obj.TryGetValue("count", out token))
            {
                return Convert.ToInt32(token.ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取features
        /// </summary>
        /// <returns></returns>
        public JArray GetFeatures(string where, string outFields = "*") {
            string queryStr = $"query?where={where}&time=&returnCountOnly=false&returnIdsOnly=false&returnGeometry=true&outFields={outFields}&f=pjson";
            string result = ZWebClient.DownloadString($"{url}/{id}/{queryStr}");
            JObject obj = JObject.Parse(result);
            JArray features = obj.Value<JArray>("features");
            return features;
        }
        /// <summary>
        /// 获取OBJECTID范围为 [MinId , MaxId) 内的features
        /// </summary>
        /// <param name="MinId"></param>
        /// <param name="MaxId"></param>
        /// <param name="outFields"></param>
        /// <returns></returns>
        public JArray GetFeaturesById(int MinId, int MaxId, string where = "", string outFields = "*")
        {
            string _where = $"{ObjectIdFieldName}>={MinId} and {ObjectIdFieldName}<{MaxId}";
            if (!where.IsNullOrWhiteSpace()) {
                _where += $" and {where}";
            }
            return GetFeatures(_where, outFields);
        }
    }
}
