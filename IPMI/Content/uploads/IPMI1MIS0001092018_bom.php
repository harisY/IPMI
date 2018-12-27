<?




if ($tgll=='')
{
$orderdate1=date("d-m-Y");
}


require_once("rahasia.php");


    $th = date("Y");
    $sql3x = "select bomid from bomh where bomid='$textfield'";
    $kon=mssql_query($sql3x);
    $ada = mssql_fetch_assoc($kon);
	
    $sql3 = "select MAX(SUBSTRING(bomid,1,7))+1 AS soakhir from bomh ";
    $kon=mssql_query($sql3);
    $akhir = mssql_fetch_assoc($kon);

if (strlen($akhir['soakhir'])==1)
{$ab='000000'.$akhir['soakhir'];}
else
if (strlen($akhir['soakhir'])==2)
{$ab='00000'.$akhir['soakhir'];}
else	
if (strlen($akhir['soakhir'])==3)
{$ab='0000'.$akhir['soakhir'];}
else
if (strlen($akhir['soakhir'])==4)
{$ab='000'.$akhir['soakhir'];}
else
if (strlen($akhir['soakhir'])==5)
{$ab='00'.$akhir['soakhir'];}
else
if (strlen($akhir['soakhir'])==6)
{$ab='0'.$akhir['soakhir'];}
else
if (strlen($akhir['soakhir'])==7)
{$ab=$akhir['soakhir'];}

	if ($akhir['soakhir']=='')
		{$aa='0000001';}
      else
         {$aa=$ab;}
if ($ada['bomid']=='')
{$ac=$aa;}
else
{$ac=$textfield;}


?>
<html>
<head>
<title>BILL OF MATERIAL</title>
<style type="text/css">
<!--
.style2 {
	font-size: 24px;
	font-weight: bold;
	font-style: italic;
}
-->
</style>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<LINK href="" rel="stylesheet" type="text/css">
<script type="text/javascript" src="formretain.js">

/***********************************************
* Recall Form Values script-  Dynamic Drive DHTML code library (www.dynamicdrive.com)
* This notice MUST stay intact for legal use
* Visit Dynamic Drive at http://www.dynamicdrive.com/ for full source code
***********************************************/

</script>
<script language="javascript">
<!--
var code;

function Konfirmasi(code)
	{
		tanya = confirm("Anda yakin akan menghapusnya?")
		if(tanya != "0") 
		{
			top.location = "delgudang.php?id="+kode;
		}

	}
function Konfirmasix(code)
	{
			top.location = "bom.php?id="+textfield;
	}
function MM_openBrWindow(theURL,winName,features) { //v2.0
  window.open(theURL,winName,features);
}

function newWindow(mypage,myname,w,h,features) {
      var winl = (screen.width-w)/2;
      var wint = (screen.height-h)/2;
      if (winl < 0) winl = 0;
      if (wint < 0) wint = 0;
      var settings = 'height=' + h + ',';
      settings += 'width=' + w + ',';
      settings += 'top=' + wint + ',';
      settings += 'left=' + winl + ',';
      settings += features;
      win = window.open(mypage,myname,settings);
      win.window.focus();
}
//-->
</SCRIPT>
<script language="javascript">
function po()
{
	form1.submit();
}

function New()
{

	document.form2.submit();	
}
function New2()
{
	document.form3.submit();	
}
</script>
 <LINK href="mystyle.css" rel="stylesheet" type="text/css">       
        <script src="development-bundle/jquery-1.8.0.min.js"></script>
        <script src="development-bundle/ui/ui.core.js"></script>
        <script src="development-bundle/ui/ui.datepicker.js"></script>
        <script src="development-bundle/ui/i18n/ui.datepicker-id.js"></script>
        <script type="text/javascript">
            $(document).ready(function(){
                $("#orderdate1").datepicker({
                    dateFormat : "dd-mm-yy",
                    changeMonth : true,
                    changeYear : true
                });
            });
        </script>
</head>
<body>
<div class="page">

		<form id="form1" name="form1" action="save_bom1.php" method="POST">
  <table width="865" border="0" cellpadding="0" cellspacing="0">
    <tr> 
      <td colspan="6"><div align="center" class="style2"><font color="#CC33FF">BILL OF MATERIAL</font></div></td>
    </tr>
    <tr> 
      <td width="19%">&nbsp;</td>
      <td width="2%"><div align="center"></div></td>
      <td width="25%">&nbsp;</td>
      <td width="13%">&nbsp;</td>
      <td width="2%">&nbsp;</td>
      <td width="39%">&nbsp;</td>
    </tr>
    <tr> 
      <td>BOM ID</td>
      <td><div align="center">:</div></td>
      <td><input name="textfield" type="text" size="20" value="<?php echo $ac ?>" >
      <a href="#" onClick="MM_openBrWindow('m_bom.php','solusi','status=yes,scrollbars=yes,width=750,height=600')"><img src="arrow.gif" width="25" height="24" border="0"/>
        <input name="kode4" type="hidden" id="kode4">
      </a></td>
      <td>Site</td>
      <td>:</td>
      <td><select name="selectsite" id="selectsite">
      <option value="<?php echo $selectsite ?>"><?php echo $selectsite ?></option>
        <option>TSC1</option>
        <option>TSC3</option>
      </select></td>
    </tr>
    <tr> 
      <td>Eff Date</td>
      <td><div align="center">:</div></td>
      <td> <input name="orderdate1" type="text" id="orderdate1" value="<?php echo $orderdate1; ?>" >
      </font></td>
      <td>Runner</td>
      <td>:</td>
      <td><input name="textfield7" type="text" size="20" value="<?php echo $textfield7 ?>" ></td>
    </tr>
    <tr>
      <td>InvtID </td>
      <td align="center">:</td>
      <td rowspan="2"><input name="textfield472" type="text" size="20" readonly value="<?php echo $textfield472 ?>" >
        <a href="#" onClick="MM_openBrWindow('m_item.php','solusi','status=yes,scrollbars=yes,width=850,height=600')"><img src="arrow.gif" width="25" height="24" border="0"/></a>
        <input name="nama1" type="hidden" id="nama1">
        <input name="kode3" type="hidden" id="kode3">
      <input name="textfield4722" type="text" size="40" readonly value="<?php echo $textfield4722 ?>" ></td>
      <td>Cycle Time</td>
      <td><div align="center">:</div></td>
      <td><input name="textfield6" type="text" size="20" value="<?php echo $textfield6 ?>" ></td>
    </tr>
    <tr>
      <td>&nbsp;</td>
      <td>&nbsp;</td>
      <td>M/C Ton</td>
      <td align="center">:</td>
      <td><input name="textfield5" type="text" size="20" value="<?php echo $textfield5 ?>" ></td>
    </tr>
    <tr> 
      <td colspan="6"><div align="center"> 
		  
        </div></td>
    </tr>
  </table>
    <?php



 //   $sql3 = "SELECT * FROM po where bomid='$textfield' order by bomid";
	$sql3 = "SELECT bomh.bomid,ltrim(rtrim(bom.invtid)) as invtid,bom.descr,CAST(bom.qty AS decimal(11,9)) as qty,bom.unit FROM bomh inner join bom on bomh.bomid=bom.bomid where bom.bomid = '$textfield' ";
    $kon=mssql_query($sql3);
	
?>
    <table class="layout display responsive-table">
    <thead>
        <tr>
            <th>NO</th>
            <th>InvtiD</th>
            <th>Nama Barang</th>
            <th>Qty</th>
            <th>Unit</th>
            <th>Pilihan</th>
</tr>
<?
     while (list($bomid,$invtid,$descr,$qty,$unit) =
                mssql_fetch_row($kon))
     {
	 $nomer=$nomer+1;
          // format dalam baris dan kolom tabel
          echo "<tr>\n";
          echo "<td>$nomer</td>\n";		  
          echo "<td>$invtid</td>\n";		  
          echo "<td>$descr</td>\n";
		  echo "<td>$qty</td>\n";
          echo "<td>$unit</td>\n";

		  echo "<td align=center>";
		  		  if ($jml==$sisa)
		  {
			  echo "<a href='#' onClick=newWindow('edit_bom.php?id=$bomid&kd=$invtid','ubah','900','340','resizable=1,scrollbars=1,status=0,toolbar=0')><img src='ubah.png' border=0 onMouseOver=showhint('Ubah Group!', this, event, '80px')></a>";
			  	echo "
<a href='#' onClick=newWindow('del_bom.php?id=$bomid&kd=$invtid','ubah','900','340','resizable=1,scrollbars=1,status=0,toolbar=0')><img src='hapus.png' border=0 onMouseOver=showhint('Ubah Group!', this, event, '80px')></a> ";
		 // echo "<a href='del_bom.php?id=$bomid&kd=$invtid' onClick='if (! Konfirmasi()) {return false;}'><img src='hapus.png' border=0></a>";
		  }
//          echo "  |  ";
 //         echo "<a href=\"editpo.php?id=$bomid\">Edit</a>";
          echo "</td>\n";
          echo "</tr>\n";
     }
     echo "</table>\n";

     // bebaskan memori yang digunakan untuk proses
     mssql_free_result($kon); 
?>
<table width="865" border="0" cellspacing="0" cellpadding="0">
<tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
</tr>
<tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
</tr>
<tr>
      <td>InvtID</td>
      <td><div align="center">:</div></td>
      <td><input name="textfield3" type="text" readonly>
      <a href="#" onClick="MM_openBrWindow('m_item2.php?id=<? echo $textfield472; ?>&cust=<? echo $textfield4722; ?>','solusi','status=yes,scrollbars=yes,width=1100,height=600')"><img src="arrow.gif" width="25" height="24" border="0"/></a>
      <input name="kode8" type="hidden" id="kode8">
      
      <input name="kode2" type="hidden" id="kode2">
      
          
      <td>&nbsp;</td>
      <td>&nbsp;</td>
      <td>&nbsp;</td>
</tr>
    <tr>
      <td>&nbsp;</td>
      <td>&nbsp;</td>
      <td><input name="textfield4" type="text" size="50" readonly></td>
      <td>&nbsp;</td>
      <td>&nbsp;</td>
      <td>&nbsp;</td>
    </tr>
    <tr>
      <td>Qty</td>
      <td><div align="center">:</div></td>
      <td><input name="textfield2" type="text" size="15">
      <input name="textfieldx2x" type="hidden" id="textfieldx2x"></td>
      <td>&nbsp;</td>
      <td>&nbsp;</td>
      <td>&nbsp;</td>
    </tr>
    <tr>
      <td>Unit</td>
      <td>&nbsp;</td>
      <td><input name="textfield10" type="text" size="10" maxlength="10" id="textfield10"></td>
      <td>&nbsp;</td>
      <td>&nbsp;</td>
      <td>&nbsp;</td>
    <tr> 
      <td colspan="6">	  
          <div align="center">
            <input type="button" name="Submit3" value="New" onClick="New2()"/>
            <input type="submit" name="tambah" value="Save" onClick="po()">
          </div></td>
    </table>
</form>

  <form id="form2" name="form2" method="post" action="bom.php" ></form> 
  <form id="form3" name="form3" method="post" action="bom.php"></form>  
  </form>
  
<p><a href="bomrouting2.php?ortu=<? echo $textfield472 ?>" target="_blank">VIEW BOM ROUTING</a>
    
</p>

<p><a href="visualbom.php" target="_blank">VIEW VISUAL BOM TSC1</a></p>
<p><a href="visualbom2.php" target="_blank">VIEW VISUAL BOM TSC3</a></p>
<p><a href="multilevelbom3a.php" target="_blank">VIEW ALL MULTILEVEL BOM</a></p>
<p><a href="whereuse.php" target="_blank">VIEW WHERE USE MATERIAL</a></p>
<p><a href="list_inventory.php" target="_blank">INSERT NEW INVENTORY ITEM</a></p>

<p>&nbsp;</p>
</div>
</body>
</html>