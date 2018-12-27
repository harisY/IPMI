<?

require_once("rahasia.php");
		$sql="delete from multilevelbom";
	$rs=mssql_query($sql);
$queryrow2="select distinct parentid from bom";
$queryrow=mssql_query($queryrow2) or die("Query Error...");
	
    while ($row = mssql_fetch_array($queryrow))
  	{
		$parent=$row['parentid'];
		$sql="insert into multilevelbom (FinishedGood, parentid, invtid, qty,Extendedqty, descr, unit,bomid) select FinishedGood, parentid, invtid, qty, sum(Extendedqty) as Extendedqty, descr, unit,bomid from dbo.fn_GetMultiLevelBOM('$parent') group by FinishedGood, parentid, invtid, qty, descr, unit,bomid";
	$rs=mssql_query($sql);
	
	}
  ?>

