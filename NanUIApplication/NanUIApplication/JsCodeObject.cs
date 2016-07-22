using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chromium;
using Chromium.Remote.Event;
using NetDimension.NanUI.ChromiumCore;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace NanUIApplication
{
    public class JsCodeObject : JSObject
    {
        private readonly NanUIMainForm _nanUIMainForm;
        public string result = "";
        internal JsCodeObject(NanUIMainForm nanUIMainForm)
        {
            this._nanUIMainForm = nanUIMainForm;
            //AddFunction("queryFunction").Execute += JsCodeObject_TestClick;
            AddFunction("setVal").Execute += JsCodeObject_ResultClick;
        }

        private void JsCodeObject_ResultClick(object sender, CfrV8HandlerExecuteEventArgs e)
        {
            var transValue = e.Arguments[0].StringValue;//有两种获取值的方法
            if (!string.IsNullOrEmpty(transValue))
            {
                string key = System.Configuration.ConfigurationSettings.AppSettings["KuaidiKey"];
              
                string requestURL = "http://www.aikuaidi.cn/rest/?key=" + key + "&order=" + transValue + "&id=" + "shunfeng" + "&ord=desc&show=json";
                result = SubmitRequest(requestURL);
            }
            var js = $"setV('{result}');";
            _nanUIMainForm.ExecuteJavascript(js);
            //MessageBox.Show("Hello!" + content);
        }


        private void JsCodeObject_TestClick(object sender, CfrV8HandlerExecuteEventArgs e)
        {
            _nanUIMainForm.EvaluateJavascript(@"getExpress()", (value, ex) =>
            {
                if (ex != null)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                string key = System.Configuration.ConfigurationSettings.AppSettings["KuaidiKey"];
                var content = value.IsString && !string.IsNullOrEmpty(value.StringValue) ? value.StringValue : "No One";
                string requestURL = "http://www.aikuaidi.cn/rest/?key=" + key + "&order=" + content + "&id=" + "shunfeng" + "&ord=desc&show=json";
                result = SubmitRequest(requestURL);

            });

        }


        public string SubmitRequest(string RequestURL)
        {
            string result = null;
            HttpWebResponse response = null;
            HttpWebRequest request = WebRequest.Create(RequestURL) as HttpWebRequest;
            request.Method = "GET";
            response = (HttpWebResponse)request.GetResponse();
            result = ReadHttpResponse(response);
            var conversionResult = new StringReader(result);
            var jsonReader = new JsonTextReader(conversionResult);
            var serializer = new JsonSerializer();
            var jsonResult = serializer.Deserialize<DataConvertModel>(jsonReader);
            string strResult = "";
            foreach (var item in jsonResult.data)
            {
                strResult += item.time + item.content + ";";
            }
            return strResult;
        }
        public string ReadHttpResponse(HttpWebResponse response)
        {
            string result = string.Empty;
            var responseStream = response.GetResponseStream();
            if (responseStream != null && (response.StatusCode == HttpStatusCode.OK && responseStream.CanRead))
            {
                result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            //XMLnode(result);
            response.Close();
            return result;
        }
    }
}
