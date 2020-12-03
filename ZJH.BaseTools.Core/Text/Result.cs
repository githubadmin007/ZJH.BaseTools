using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJH.BaseTools.IO;

namespace ZJH.BaseTools.Text
{
    public class Result
    {
        public static Result Success = CreateSuccess("成功", null);
        public static Result Defeat = CreateDefeat("失败", null);

        /// <summary>
        /// 状态。
        /// </summary>
        public int code;
        /// <summary>
        /// 说明文字
        /// </summary>
        public string msg;
        /// <summary>
        /// 数据
        /// </summary>
        public object data;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public Result(int code, string msg, object data = null)
        {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public Result(ResultCode code, string msg, object data = null)
        {
            this.code = (int)code;
            this.msg = msg;
            this.data = data;
        }
        /// <summary>
        /// 设置成功信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public void SetSuccessMsg(string msg, object data = null)
        {
            this.code = (int)ResultCode.Success;
            this.msg = msg;
            this.data = data;
        }
        /// <summary>
        /// 设置失败信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public void SetDefeatMsg(string msg, object data = null)
        {
            this.code = (int)ResultCode.Defeat;
            this.msg = msg;
            this.data = data;
        }
        /// <summary>
        /// 根据Exception创建失败信息
        /// </summary>
        /// <param name="ex"></param>
        public void SetException(Exception ex)
        {
            code = (int)ResultCode.Defeat;
            msg = ex.Message;
            data = ex;
        }
        [Obsolete("建议使用ToJson")]
        public override string ToString()
        {
            var result = new
            {
                code = code,
                msg = msg,
                data = data
            };
            return JsonConvert.SerializeObject(result);
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static Result Parse(string json)
        {
            try
            {
                JObject obj = JObject.Parse(json);
                int code = int.Parse(obj.GetValue("code").ToString());
                string msg = obj.GetValue("msg").ToString();
                object data = obj.GetValue("data").Type == JTokenType.Null ? null : obj.GetValue("data");
                return new Result(code, msg, data);
            }
            catch (Exception ex)
            {
                Logger.log("Result.Parse", ex);
                //throw new Exception("传入了不符合BaseResult的格式", ex);
            }
            return new Result(ResultCode.Defeat, "传入了不符合BaseResult的格式");
        }

        public static Result CreateSuccess(string msg,object data = null) {
            return new Result(ResultCode.Success, msg, data);
        }
        public static Result CreateDefeat(string msg, object data = null)
        {
            return new Result(ResultCode.Defeat, msg, data);
        }
        public static Result CreateFromException(Exception ex)
        {
            return CreateDefeat(ex.Message, ex);
        }
    }

    public enum ResultCode
    {
        Success = 200,// 成功
        Defeat = 400,// 失败(可以继续细分出更多类型)
        NotFound = 404,// 请求的资源不存在
        // 5开头的为用户登陆相关
        UserNotExist = 501,// 用户不存在
        UserPwError = 502,// 用户密码错误
        NotLogin = 503,// 未登录状态，跳转登录页
        LoginTimeout = 504,// 登录过期，请重新登录
        // 6开头的为数据库相关
        DB_Timeout = 601,// 数据库连接超时
        DB_InsertFail = 602, //数据库插入失败
        DB_DeleteFail = 603, //数据库删除失败
        DB_SelectFail = 604, //数据库查询失败
        DB_UpdateFail = 605, //数据库更新失败
        SQLInjection = 606, //SQL注入
    }

    /// <summary>
    /// 接口返回状态码扩展类
    /// </summary>
    static public class ResultCodeEx
    {
        /// <summary>
        /// 创建接口返回数据对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Result GetResult(this ResultCode code, string msg = "", object data = null)
        {
            return new Result(code, msg, data);
        }

        /// <summary>
        /// 创建接口返回数据对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Result GetResult(this ResultCode code, object data)
        {
            return new Result(code, "", data);
        }
    }
}
