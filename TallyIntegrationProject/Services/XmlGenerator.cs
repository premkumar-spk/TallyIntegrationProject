using System.Text.RegularExpressions;

namespace TallyIntegrationProject.Services
{
    public class XmlGenerator
    {

        public string CreateLedgerXML(string ledgerName, string parent)
        {
            return $@"
<ENVELOPE>
 <HEADER>
  <TALLYREQUEST>Import Data</TALLYREQUEST>
 </HEADER>
 <BODY>
  <IMPORTDATA>
   <REQUESTDESC>
    <REPORTNAME>All Masters</REPORTNAME>
   </REQUESTDESC>
   <REQUESTDATA>
    <TALLYMESSAGE>
     <LEDGER NAME=""{ledgerName}"" ACTION=""Create"">
        <NAME.LIST>
                <NAME>{ledgerName}</NAME>
            </NAME.LIST>
       <PARENT>{parent}</PARENT>
        <ISBILLWISEON>Yes</ISBILLWISEON>
     </LEDGER>
    </TALLYMESSAGE>
   </REQUESTDATA>
  </IMPORTDATA>
 </BODY>
</ENVELOPE>";
        }


        public string CreateStockXML(string name, string unit, string? group)
        {
            return $@"
<ENVELOPE>
 <HEADER>
  <TALLYREQUEST>Import Data</TALLYREQUEST>
 </HEADER>

 <BODY>
  <IMPORTDATA>
   <REQUESTDESC>
    <REPORTNAME>All Masters</REPORTNAME>
   </REQUESTDESC>

   <REQUESTDATA>
    <TALLYMESSAGE xmlns:UDF='TallyUDF'>

     <STOCKITEM NAME='{name}' ACTION='Create'>
        <NAME>{name}</NAME>
        <PARENT>{group}</PARENT>
        <BASEUNITS>{unit}</BASEUNITS>
     </STOCKITEM>

    </TALLYMESSAGE>
   </REQUESTDATA>
  </IMPORTDATA>
 </BODY>
</ENVELOPE>";
        }


        public string CreateSalesVoucherXML(string companyname,string voucherdate, string customerledger, string itemName, decimal quantity, decimal rate, decimal amount)
        {
            return
                $@"<ENVELOPE>
    <HEADER>
        <TALLYREQUEST>Import Data</TALLYREQUEST>
    </HEADER>
    <BODY>
        <IMPORTDATA>

            <REQUESTDESC>
                <REPORTNAME>Vouchers</REPORTNAME>
                <STATICVARIABLES>
                    <SVCURRENTCOMPANY>{companyname}</SVCURRENTCOMPANY>
                </STATICVARIABLES>
            </REQUESTDESC>

            <REQUESTDATA>
                <TALLYMESSAGE xmlns:UDF=""TallyUDF"">

                 < VOUCHER VCHTYPE = ""Sales"" ACTION = ""Create"" >

                        <DATE>{voucherdate}</DATE>
                        <VOUCHERTYPENAME>Sales</VOUCHERTYPENAME>
                        <PARTYLEDGERNAME>{customerledger}</PARTYLEDGERNAME>
                        <ISINVOICE>Yes</ISINVOICE>

                        <ALLINVENTORYENTRIES.LIST>
                            <STOCKITEMNAME>{itemName}</STOCKITEMNAME>
                            <RATE>{rate}</RATE>
                            <AMOUNT>{amount}</AMOUNT>
                            <ACTUALQTY>{quantity}</ACTUALQTY>
                            <BILLEDQTY>{quantity}</BILLEDQTY>
                        </ALLINVENTORYENTRIES.LIST>

                        <ALLLEDGERENTRIES.LIST>
                            <LEDGERNAME>{customerledger}</LEDGERNAME>
                            <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>
                            <AMOUNT>-{amount}</AMOUNT>
                        </ALLLEDGERENTRIES.LIST>

                        <ALLLEDGERENTRIES.LIST>
                            <LEDGERNAME>Sales</LEDGERNAME>
                            <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>
                            <AMOUNT>{amount}</AMOUNT>
                        </ALLLEDGERENTRIES.LIST>

                    </VOUCHER>
                </TALLYMESSAGE>
            </REQUESTDATA>
        </IMPORTDATA>
    </BODY>
</ENVELOPE>";
        }


        public string CheckMasterExistsXML(string masterName, string masterType)
        { 
            return $@"<ENVELOPE>
<HEADER><TALLYREQUEST>Export Data</TALLYREQUEST></HEADER>
<BODY>
  <EXPORTDATA>
    <REQUESTDESC>
      <REPORTNAME>List of Masters</REPORTNAME>
      <STATICVARIABLES>
        <SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT>
        <MSTNAME>{masterName}</MSTNAME>
      </STATICVARIABLES>
    </REQUESTDESC>
    <REQUESTDATA>
      <TALLYMESSAGE xmlns:UDF='TallyUDF'>
         </TALLYMESSAGE>
    </REQUESTDATA>
  </EXPORTDATA>
</BODY>
</ENVELOPE>";
        }
        public string CreateLedgerXML(string company, string ledgerName, string parentGroup)
        {
            return $@"<ENVELOPE>
<HEADER>
    <TALLYREQUEST>Import Data</TALLYREQUEST>
</HEADER>
<BODY>
    <IMPORTDATA>
        <REQUESTDESC>
            <REPORTNAME>All Masters</REPORTNAME>
            <STATICVARIABLES>
                <SVCURRENTCOMPANY>{company}</SVCURRENTCOMPANY>
            </STATICVARIABLES>
        </REQUESTDESC>
        <REQUESTDATA>
            <TALLYMESSAGE xmlns:UDF='TallyUDF'>
                <LEDGER NAME="" ACTION=""Create"">
                    <NAME.LIST>
                        <NAME>{ledgerName}</NAME>
                    </NAME.LIST>
                    <PARENT>{parentGroup}</PARENT>
                    <ISBILLWISEON>Yes</ISBILLWISEON>   <!-- optional but useful for debtors -->
                </LEDGER>
            </TALLYMESSAGE>
        </REQUESTDATA>
    </IMPORTDATA>
</BODY>
</ENVELOPE>";
        }
    }
}