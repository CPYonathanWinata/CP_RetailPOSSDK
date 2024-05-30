/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Microsoft.Dynamics.Retail.Pos.Customer
{
    /// <summary>
    /// Customer affiliations.
    /// </summary>
    [Serializable]
    [XmlRoot("RetailCustAffiliations")]
    public class CustAffiliation
    {
        public CustAffiliation()
        {
            CustAffiliationItems = new Collection<RetailCustAffiliation>();
        }

        /// <summary>
        /// Collection of affiliation elements
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Non-read-only collection required for serialization.")]
        [XmlElement("RetailCustAffiliation")]
        public Collection<RetailCustAffiliation> CustAffiliationItems { get; set; }

        /// <summary>
        /// Convert CustAffiliation into XML
        /// </summary>
        public string ToXml()
        {
            string xmlString = string.Empty;
            XmlSerializer serializer = new XmlSerializer(typeof(CustAffiliation));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                writer.Flush();
                xmlString = writer.ToString();
            }

            return xmlString;
        }

        /// <summary>
        /// Create CustAffiliation from XML.
        /// </summary>
        /// <param name="xml">Xml string.</param>
        /// <returns>CustAffiliation object.</returns>
        public static CustAffiliation FromXml(string xml)
        {
            CustAffiliation affiliationInfo;
            XmlSerializer serializer = new XmlSerializer(typeof(CustAffiliation));
            using (StringReader reader = new StringReader(xml))
            {
                affiliationInfo = (CustAffiliation)serializer.Deserialize(reader);
                return affiliationInfo;
            }
        }
    }

    /// <summary>
    /// Customer affiliation information.
    /// </summary>
    [Serializable]
    [XmlType("RetailCustAffiliation")]
    public class RetailCustAffiliation
    {
        /// <summary>
        /// RecId (for serialization).
        /// </summary>
        [XmlAttribute("RecId")]
        public string RecIdString { get; set; }

        /// <summary>
        /// Customer account number (for serialization).
        /// </summary>
        [XmlAttribute("CustAccountNum")]
        public string CustomerId { get; set; }

        /// <summary>
        /// AffiliationId (for serialization).
        /// </summary>
        [XmlAttribute("RetailAffiliationId")]
        public string AffiliationIdString { get; set; }

        /// <summary>
        /// Record id.
        /// </summary>
        [XmlIgnore]
        public long RecId { get; private set; }

        /// <summary>
        /// Affiliation id.
        /// </summary>
        [XmlIgnore]
        public long AffiliationId { get; private set; }

        /// <summary>
        /// Parse out strongly typed values from the serialized strings
        /// </summary>
        public void Parse()
        {
            this.RecId = long.Parse(this.RecIdString);
            this.AffiliationId = long.Parse(this.AffiliationIdString);
        }
    }
}
