// 공시정보:: 공시검색 | List
// https://opendart.fss.or.kr/guide/detail.do?apiGrpCd=DS001&apiId=2019001

using System;
using System.Collections.ObjectModel;
using System.Xml;

namespace OpenDARTCSharp
{
    public sealed class ListPageData
    {
        public readonly int PageNumber;
        public readonly int PageCount;
        public readonly int TotalCount;
        public readonly int TotalPage;

        public ListPageData(int page_no, int page_count, int total_count, int total_page)
        {
            this.PageNumber = page_no;
            this.PageCount = page_count;
            this.TotalCount = total_count;
            this.TotalPage = total_page;
        }

        public override string ToString()
        {
            return OpenDartApi.ToString<ListPageData>(this);
        }
    }
    public sealed class ListItemData
    {
        public CompanyData.CORP_CLASS CorporationClass;
        public CorpCodeData CorporationCode;
        public string ReportName;
        [Format("D14")]
        public long ReceiptNumber;
        public string FilerName;
        [Format("yyyyMMdd")]
        public DateTime ReceiptDate;
        public string Remark;

        public override string ToString()
        {
            return OpenDartApi.ToString<ListItemData>(this);
        }
    }
    public sealed class ListData
    {
        public readonly ListPageData page;
        public readonly ReadOnlyCollection<ListItemData> items;
    }

    partial class OpenDartApi
    {
        public static void LoadList(
            string apiKey,
            DateTime beginDate = default(DateTime),
            DateTime endDate = default(DateTime),
            CompanyData.CORP_CLASS corpClass = default(CompanyData.CORP_CLASS),
            int pageNumber = default(int),
            int pageCount = default(int)
        )
        {
            string url = $"{ApiBaseUrl}list.xml?" +
                        $"crtfc_key={apiKey}&";
            if (beginDate != default(DateTime))
                url += $"bgn_de={beginDate:yyyyMMdd}&";
            if (endDate != default(DateTime))
                url += $"end_de={endDate:yyyyMMdd}&";
            if (corpClass != default(CompanyData.CORP_CLASS))
                url += $"corp_cls={corpClass}&";
            if (pageNumber != default(int))
                url += $"page_no={pageNumber}&";
            if (pageCount != default(int))
                url += $"page_count={pageCount}";
            var xml = DownloadXml(url);

            var resultNode = xml.SelectSingleNode("result");
            Validate(resultNode);

            int page_no = int.Parse(resultNode.SelectSingleNode("page_no").InnerText);
            int page_count = int.Parse(resultNode.SelectSingleNode("page_count").InnerText);
            int total_count = int.Parse(resultNode.SelectSingleNode("total_count").InnerText);
            int total_page = int.Parse(resultNode.SelectSingleNode("total_page").InnerText);

            ListPageData pageData = new ListPageData(page_no, page_count, total_count, total_page);
            Console.WriteLine(pageData);

            var listNodes = resultNode.SelectNodes("list");
            ListItemData[] items = new ListItemData[listNodes.Count];
            int index = 0;
            foreach (XmlNode listNode in listNodes)
            {
                ListItemData itemData = new ListItemData();
                itemData.CorporationClass = (CompanyData.CORP_CLASS)Enum.Parse(typeof(CompanyData.CORP_CLASS), listNode.SelectSingleNode("corp_cls").InnerText);
                itemData.CorporationCode = CorpCodeData.Parse(listNode);
                itemData.ReportName = listNode.SelectSingleNode("report_nm").InnerText;
                itemData.ReceiptNumber = long.Parse(listNode.SelectSingleNode("rcept_no").InnerText);
                itemData.FilerName = listNode.SelectSingleNode("flr_nm").InnerText;
                itemData.ReceiptDate = Util.Parse_yyyyMMdd(listNode.SelectSingleNode("rcept_dt").InnerText);
                itemData.Remark = listNode.SelectSingleNode("rm").InnerText;
                Console.WriteLine(itemData);
                items[index++] = itemData;
            }
        }
    }
}