using System.Collections.Generic;
using System.IO;

namespace Xml2CSharp
{
    public class ClassInfoWriter
    {
        private readonly IEnumerable<Class> _classInfo;

        public ClassInfoWriter(IEnumerable<Class> classInfo)
        {
            _classInfo = classInfo;
        }

        public void Write(TextWriter textWriter, string @namespace = null)
        {
            using (textWriter)
            {
                textWriter.WriteLine("using System;");
                textWriter.WriteLine("using System.Collections.Generic;");
                textWriter.WriteLine("using System.Xml.Serialization;");
                textWriter.WriteLine("");

                if (!string.IsNullOrEmpty(@namespace))
                {
                    textWriter.WriteLine("namespace {0}", @namespace);
                    textWriter.WriteLine("{");
                    foreach (var @class in _classInfo)
                    {
                        textWriter.WriteLine("\t[XmlRoot(ElementName=\"{0}\", Namespace=\"{1}\")]", @class.XmlName, @class.Namespace);
                        textWriter.WriteLine("\tpublic class {0} {{", @class.Name);
                        foreach (var field in @class.Fields)
                        {
                            var fieldName = @class.Name == field.Name ? field.Name + "1" : field.Name;
                            textWriter.WriteLine("\t\t[Xml{0}({0}Name=\"{1}\", Namespace=\"{2}\")]", field.XmlType, field.XmlName, field.Namespace);
                            textWriter.WriteLine("\t\tpublic {0} {1} {{ get; set; }}", field.Type, fieldName);
                        }
                        textWriter.WriteLine("\t}");
                        textWriter.WriteLine("");
                    }
                    textWriter.WriteLine("}");
                }
                else
                {
                    foreach (var @class in _classInfo)
                    {
                        textWriter.WriteLine("[XmlRoot(ElementName=\"{0}\", Namespace=\"{1}\")]", @class.XmlName, @class.Namespace);
                        textWriter.WriteLine("public class {0} {{", @class.Name);
                        foreach (var field in @class.Fields)
                        {
                            var fieldName = @class.Name == field.Name ? field.Name + "1" : field.Name;
                            textWriter.WriteLine("\t[Xml{0}({0}Name=\"{1}\", Namespace=\"{2}\")]", field.XmlType, field.XmlName, field.Namespace);
                            textWriter.WriteLine("\tpublic {0} {1} {{ get; set; }}", field.Type, fieldName);
                        }
                        textWriter.WriteLine("}");
                        textWriter.WriteLine("");
                    }
                }
            }
        }
    }
}
