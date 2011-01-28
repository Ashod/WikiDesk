namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    [Serializable]
    public class WikiMagicWordConstants
    {
        public static WikiMagicWordConstants Deserialize(string filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(WikiMagicWordConstants));
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return (WikiMagicWordConstants)serializer.Deserialize(fs);
                }
            }
            catch (Exception)
            {
            }

            return new WikiMagicWordConstants();
        }

        public void Serialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(WikiMagicWordConstants));
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(fs, this);
            }
        }

        public List<string> VariablesIds = new List<string>();

        public List<string> DoubleUnderscoreIds = new List<string>();
    }
}
