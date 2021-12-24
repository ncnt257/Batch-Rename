using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Batch_Rename
{
    class RenameRuleFactory
    {
        public Dictionary<string, IStringOperation> Prototypes = new Dictionary<string, IStringOperation>();

        public RenameRuleFactory(MainWindow mainWindow)
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            var fis = new DirectoryInfo(exePath).GetFiles("*.dll");


            foreach (var f in fis) // Lần lượt duyệt qua các file dll
            {
                var assembly = Assembly.LoadFile(f.FullName);
                var types = assembly.GetTypes();
                foreach (var t in types)
                {

                    if (t.IsClass && typeof(IStringOperation).IsAssignableFrom(t))
                    {
                        IStringOperation c = (IStringOperation)Activator.CreateInstance(t);//do các luật có hàm tạo, new Args rồi nên không cần load class Args như trước nữa
                        c.PreviewTriggerEvent += mainWindow.PreviewTrigger;
                        Prototypes.Add(c.Name, c);//Replace, AddPrefix,...
                    }
                }
            }
        }
        public IStringOperation Create(RawRule input)
        {
            var result = Prototypes[input.RuleName].Clone();
            result.CreateFromRaw(input);
            return result;
        }
    }
}
