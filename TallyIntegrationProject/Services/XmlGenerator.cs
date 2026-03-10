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


        public string CreateSalesVoucherXML(string customer, string item, decimal amount)
        {
            return $@"
<ENVELOPE>
 <HEADER>
  <TALLYREQUEST>Import Data</TALLYREQUEST>
 </HEADER>
 <BODY>
  <IMPORTDATA>
   <REQUESTDATA>
    <TALLYMESSAGE>
     <VOUCHER VCHTYPE=""Sales"" ACTION=""Create"">
      <PARTYLEDGERNAME>{customer}</PARTYLEDGERNAME>
      <ALLINVENTORYENTRIES.LIST>
       <STOCKITEMNAME>{item}</STOCKITEMNAME>
       <RATE>{amount}</RATE>
       <AMOUNT>{amount}</AMOUNT>
      </ALLINVENTORYENTRIES.LIST>
     </VOUCHER>
    </TALLYMESSAGE>
   </REQUESTDATA>
  </IMPORTDATA>
 </BODY>
</ENVELOPE>";
        }

    }
}