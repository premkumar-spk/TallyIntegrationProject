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
      <PARENT>{parent}</PARENT>
     </LEDGER>
    </TALLYMESSAGE>
   </REQUESTDATA>
  </IMPORTDATA>
 </BODY>
</ENVELOPE>";
        }


        public string CreateStockXML(string name, string unit)
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
     <STOCKITEM NAME=""{name}"" ACTION=""Create"">
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