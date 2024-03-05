// 공시정보:: 고유번호 | CorpCode
// https://opendart.fss.or.kr/guide/detail.do?apiGrpCd=DS001&apiId=2019018

#pragma warning disable IDE0003
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace OpenDARTCSharp
{
    public struct CorpCodeData
    {
        #region Fields
        /// <summary> 고유번호 </summary>
        [Format("D8")]
        public readonly int CorpCode;

        /// <summary> 종목코드 </summary>
        [Format("D6")]
        public readonly int StockCode;

        /// <summary> 정식명칭 </summary>
        public readonly string CorpName;
        #endregion

        public CorpCodeData(int corpCode, string corpName, int stockCode)
        {
            this.CorpCode = corpCode;
            this.CorpName = corpName;
            this.StockCode = stockCode;
        }

        public override string ToString()
        {
            return OpenDartApi.ToString<CorpCodeData>(this);
        }

        internal static CorpCodeData Parse(XmlNode node)
        {
            string corp_code = node.SelectSingleNode("corp_code").InnerText;
            string corp_name = node.SelectSingleNode("corp_name").InnerText;
            string stock_code = node.SelectSingleNode("stock_code").InnerText;
            return new CorpCodeData(
                int.Parse(corp_code),
                corp_name,
                string.IsNullOrWhiteSpace(stock_code) ? 0 : int.Parse(stock_code)
            );
        }
    }

    partial class OpenDartApi
    {
        private static void ValidateCorpCode(byte[] xmlBytes)
        {
            XmlDocument doc = xmlBytes.ToXml();
            var resultNode = doc.SelectSingleNode("result");
            Validate(resultNode);
        }

        /// <exception cref="DartException"/>
        private static void DownloadCorpCode(string apiKey)
        {
            const string CorpCodeApiEndpoint = "corpCode.xml";
            string url = $@"{ApiBaseUrl}{CorpCodeApiEndpoint}?crtfc_key={apiKey}";

            byte[] data = DownloadData(url);

            if (!data.IsZIP())
                ValidateCorpCode(data);

            string fileName;
            File.WriteAllBytes(fileName = Util.GetNextFileName() + ".zip", data);

            ZipFile.ExtractToDirectory(fileName, @".\");
            File.Delete(fileName);
        }
        private static ReadOnlyCollection<CorpCodeData> ParseCorpCodesList(XmlDocument xml)
        {
            var nodes = xml.SelectNodes("result/list");
            int nNodes = nodes.Count;
            int index = 0;
            CorpCodeData[] res = new CorpCodeData[nNodes];
            foreach (XmlNode node in nodes)
                res[index++] = CorpCodeData.Parse(node);
            return Array.AsReadOnly(res);
        }
        /// <exception cref="DartException"/>
        public static ReadOnlyCollection<CorpCodeData> LoadCorpCodes(string apiKey, bool forcelyUpdate = false)
        {
            const string xmlFileName = "CORPCODE.xml";

            bool existsXmlFile;
            if (!(existsXmlFile = File.Exists(xmlFileName)) || forcelyUpdate)
            {
                if (existsXmlFile)
                    File.Delete(xmlFileName);

                DownloadCorpCode(apiKey);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFileName);
            return ParseCorpCodesList(doc);
        }
    }
}
