
namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    using WikiDesk.Core;

    [Serializable]
    public class WikiDomains
    {
        public static WikiDomains DefaultDomains()
        {
            WikiDomains domains = new WikiDomains();
            domains.Domains.Add(new WikiDomain("wikipedia"));
            domains.Domains.Add(new WikiDomain("wiktionary"));

            return domains;
        }

        public static WikiDomains Deserialize(string filename)
        {
            try
            {
                WikiDomains domains;
                XmlSerializer serializer = new XmlSerializer(typeof(WikiDomains));
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    domains = (WikiDomains)serializer.Deserialize(fs);
                }

                if (domains.Domains != null && domains.Domains.Count > 0)
                {
                    return domains;
                }
            }
            catch (Exception)
            {
            }

            return DefaultDomains();
        }

        public void Serialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(WikiDomains));
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(fs, this);
            }
        }

        public WikiDomain FindByName(string currentDomainName)
        {
            currentDomainName = currentDomainName.ToUpperInvariant();
            foreach (WikiDomain wikiDomain in Domains)
            {
                if (wikiDomain.Name.ToUpperInvariant() == currentDomainName)
                {
                    return wikiDomain;
                }
            }

            return null;
        }

        public readonly List<WikiDomain> Domains = new List<WikiDomain>(16);
    }
}
