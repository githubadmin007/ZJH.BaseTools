using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJH.BaseTools.Net;

namespace ZJH.EsriGIS.Server
{
    public class MapServerHelper
    {
        string MapServerUrl = "";
        List<MapServerLayer> LayerLst = new List<MapServerLayer>();
        public MapServerHelper(string url) {
            MapServerUrl = url;
            string result = ZWebClient.DownloadString($"{MapServerUrl}?f=pjson");
            JObject info = JObject.Parse(result);
            JArray layers = info.GetValue("layers") as JArray;
            LayerLst = layers.Select(obj => new MapServerLayer(MapServerUrl, (JObject)obj)).ToList();
        }
        /// <summary>
        /// 通过id查找图层
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MapServerLayer GetLayerById(int id) {
            return LayerLst.FirstOrDefault(l => l.id == id);
        }
        /// <summary>
        /// 通过名称查找图层
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MapServerLayer GetLayerByName(string name)
        {
            return LayerLst.FirstOrDefault(l => l.name == name);
        }
    }
}
