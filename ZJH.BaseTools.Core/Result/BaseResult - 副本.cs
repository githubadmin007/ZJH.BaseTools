using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using ZJH.BaseTools.IO;

namespace ZJH.BaseTools.Result
{
    public struct BaseResult
    {
        public static BaseResult SuccessResult = new BaseResult(BaseCode.Success, "成功", null);
        public static BaseResult DefeatResult = new BaseResult(BaseCode.Defeat, "失败", null);
        public int code;
        public string msg;
        public object data;
        public BaseResult(int code,string msg, object data= null) {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
        public BaseResult(BaseCode code, string msg, object data = null)
        {
            this.code = (int)code;
            this.msg = msg;
            this.data = data;
        }
        public string ToJson()
        {
            string json = JsonConvert.SerializeObject(this);
            return json;
        }
        public static BaseResult Parse(string str) {
            try
            {
                JObject obj = JObject.Parse(str);
                int code = int.Parse(obj.GetValue("code").ToString());
                string msg = obj.GetValue("msg").ToString();
                object data = obj.GetValue("data");
                return new BaseResult(code, msg, data);
            }
            catch (Exception ex) {
                Logger.log("BaseResult.Parse", ex);
                //throw new Exception("传入了不符合BaseResult的格式", ex);
            }
            return new BaseResult(-1, "传入了不符合BaseResult的格式");
        }
        public static BaseResult CreateFromException(Exception ex) {
            return new BaseResult(BaseCode.Defeat, ex.Message, ex);
        }
    }
}
