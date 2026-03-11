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


        public string CreateSalesVoucherXML(string customer, string item, decimal amount, string tallyDate)
        {
            return $@"
<ENVELOPE>
<HEADER>
<TALLYREQUEST>Import Data</TALLYREQUEST>
</HEADER>

<BODY>
<IMPORTDATA>
<REQUESTDESC>
<REPORTNAME>Vouchers</REPORTNAME>
</REQUESTDESC>

<REQUESTDATA>
<TALLYMESSAGE xmlns:UDF='TallyUDF'>

<VOUCHER VCHTYPE='Sales' ACTION='Create'>

<DATE>{tallyDate}</DATE>
<EFFECTIVEDATE>{tallyDate}</EFFECTIVEDATE>

<VOUCHERTYPENAME>Sales</VOUCHERTYPENAME>
<PARTYLEDGERNAME>{customer}</PARTYLEDGERNAME>
<ISOPTIONAL>No</ISOPTIONAL>
<ISINVOICE>Yes</ISINVOICE>

<ALLINVENTORYENTRIES.LIST>
<STOCKITEMNAME>{item}</STOCKITEMNAME>
<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>
<RATE>{amount}/Nos</RATE>
<AMOUNT>{amount}</AMOUNT>
<ACTUALQTY>1 Nos</ACTUALQTY>
<BILLEDQTY>1 Nos</BILLEDQTY>
</ALLINVENTORYENTRIES.LIST>

<ALLLEDGERENTRIES.LIST>
<LEDGERNAME>{customer}</LEDGERNAME>
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
    }
}