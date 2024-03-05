// 공시정보:: 기업개황 | Company
// https://opendart.fss.or.kr/guide/detail.do?apiGrpCd=DS001&apiId=2019002

using System;

namespace OpenDARTCSharp
{
    public sealed class CompanyData
    {
        #region Enums
        public enum CORP_CLASS : byte
        {
            None,
            /// <summary> 기타 </summary>
            E,
            /// <summary> 유가 </summary>
            Y,
            /// <summary> 코스닥 </summary>
            K,
            /// <summary> 코넥스 </summary>
            N,
        }
        #endregion

        #region Fields
        /// <summary> 고유번호 </summary>
        public readonly CorpCodeData CorpCode;

        /// <summary> 영문정식회사명칭 </summary>
        public readonly string CorpNameEng;

        /// <summary> 종목명(상장사) 또는 약식명칭(기타법인) </summary>
        public readonly string StockName;

        /// <summary> 대표자명 </summary>
        public readonly string CeoName;

        /// <summary> 법인구분, Y(유가), K(코스닥), N(코넥스), E(기타) </summary>
        public readonly CORP_CLASS CorpClass;

        /// <summary> 법인등록번호 </summary>
        public readonly string JurisdictionNo;

        /// <summary> 사업자등록번호 </summary>
        public readonly string BusinessNo;

        /// <summary> 주소 </summary>
        public readonly string Address;

        /// <summary> 홈페이지 </summary>
        public readonly string HomepageURL;

        /// <summary> IR홈페이지 </summary>
        public readonly string IRUrl;

        /// <summary> 전화번호 </summary>
        public readonly string PhoneNumber;

        /// <summary> 팩스번호 </summary>
        public readonly string FaxNumber;

        /// <summary> 업종코드 </summary>
        public readonly string IndustryCode;

        /// <summary> 설립일 </summary>
        public DateTime EstablishmentDate;

        /// <summary> 결산월 </summary>
        public readonly byte AccountingMonth;
        #endregion

        #region Constructors
        public CompanyData(
            CorpCodeData corpCode,
            string corpNameEng,
            string stockName,
            string ceo_name,
            string corp_class,
            string jurir_no,
            string bizr_no,
            string address,
            string hm_url,
            string ir_url,
            string phoneNumber,
            string faxNumber,
            string induty_code,
            string est_dt,
            string acc_mt
            )
        {
            this.CorpCode = corpCode;

            this.CorpNameEng = corpNameEng;
            this.StockName = stockName;

            this.CeoName = ceo_name;

            this.CorpClass = (CORP_CLASS)Enum.Parse(typeof(CORP_CLASS), corp_class);
            this.JurisdictionNo = jurir_no;
            this.BusinessNo = bizr_no;

            this.Address = address;

            this.HomepageURL = hm_url;
            this.IRUrl = ir_url;

            this.PhoneNumber = phoneNumber;
            this.FaxNumber = faxNumber;

            this.IndustryCode = induty_code;

            this.EstablishmentDate = est_dt.Parse_yyyyMMdd();
            this.AccountingMonth = byte.Parse(acc_mt);
        }
        #endregion

        #region override Object
        public override string ToString()
        {
            return OpenDartApi.ToString<CompanyData>(this);
        }
        #endregion
    }

    partial class OpenDartApi
    {
        public static CompanyData LoadCompany(string apiKey, CorpCodeData corpCode)
        {
            const string CompanyApiEndpoint = "company.xml";
            string url = $@"{ApiBaseUrl}{CompanyApiEndpoint}?crtfc_key={apiKey}&corp_code={corpCode.CorpCode:D8}";
            var xml = DownloadXml(url);

            var resultNode = xml.SelectSingleNode("result");
            Validate(resultNode);

            string corp_name_eng = resultNode.SelectSingleNode("corp_name_eng").InnerText;
            string stock_name = resultNode.SelectSingleNode("stock_name").InnerText;
            string ceo_nm = resultNode.SelectSingleNode("ceo_nm").InnerText;
            string corp_cls = resultNode.SelectSingleNode("corp_cls").InnerText;
            string jurir_no = resultNode.SelectSingleNode("jurir_no").InnerText;
            string bizr_no = resultNode.SelectSingleNode("bizr_no").InnerText;
            string adres = resultNode.SelectSingleNode("adres").InnerText;
            string hm_url = resultNode.SelectSingleNode("hm_url").InnerText;
            string ir_url = resultNode.SelectSingleNode("ir_url").InnerText;
            string phn_no = resultNode.SelectSingleNode("phn_no").InnerText;
            string fax_no = resultNode.SelectSingleNode("fax_no").InnerText;
            string induty_code = resultNode.SelectSingleNode("induty_code").InnerText;
            string est_dt = resultNode.SelectSingleNode("est_dt").InnerText;
            string acc_mt = resultNode.SelectSingleNode("acc_mt").InnerText;
            return new CompanyData(corpCode, corp_name_eng, stock_name, ceo_nm, corp_cls, jurir_no, bizr_no, adres, hm_url, ir_url, phn_no, fax_no, induty_code, est_dt, acc_mt);
        }
    }
}