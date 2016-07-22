using NetDimension.NanUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chromium;
using Chromium.Event;

namespace NanUIApplication
{
    public partial class NanUIMainForm : HtmlUIForm
    {
        public NanUIMainForm()
            : base("embedded://www/index.html", true)
        {
            InitializeComponent();
            LoadHandler.OnLoadEnd += LoadHandler_OnLoadEnd;

            GlobalObject.Add("hostTest", new JsCodeObject(this));
        }
        private void LoadHandler_OnLoadEnd(object sender, CfxOnLoadEndEventArgs args)
        {
            //判断下触发的事件是不是主框架的
            if (args.Frame.IsMain)
            {
                //执行JS，将当前的CEF运行版本等信息通过JS加载到网页上
                //var js = $"$client.setRuntimeInfo({{" +
                //         $" api: ['{CfxRuntime.ApiHash(0)}', '{CfxRuntime.ApiHash(1)}']," +
                //         $" cef:'{CfxRuntime.GetCefVersion()}', " +
                //         $"chrome:'{CfxRuntime.GetChromeVersion()}'," +
                //         $"os:'{CfxRuntime.PlatformOS}', " +
                //         $"arch:'{CfxRuntime.PlatformArch}'}});";
                //var ver = CfxRuntime.GetCefVersion();
                //var os = CfxRuntime.PlatformOS + " " + CfxRuntime.PlatformArch;
                //var user = Environment.UserName;
                var js = "loadinfo('{ver}','{os}','{user}');";
                ExecuteJavascript(js);
            }
        }
    }
}
