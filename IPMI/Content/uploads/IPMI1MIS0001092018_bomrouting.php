<?
require_once("rahasia.php");
?>

<?
function build_billofmaterials($output='',$pid='$pidx'){ 
static $i = 0; 
$level = "level".$i; 

${$level} = mssql_query("select invtid as id,SUBSTRING(invtid,1,17) as invtid,descr,qty,unit,parentid,'1' as child from bomrouting where parentid = '$pid'") or die("Failed");

do{ 
$level = "level".$i; 

while ($row = mssql_fetch_array(${$level})) { 

    if ($row[parentid] != '$pidx') {
        $indent = str_repeat("--------",$i); 
		// $indent = $level;
    } else {
        $indent = ""; 
//		$indent = "level1";
    }

 //   $output .= $indent.$row[groupid]."-".$row[invtid]."-".$row[revision]." - ".$row[descr]."<br>"; 
        $output .= $indent."&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;".$row[invtid]."&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;".$row[descr]."&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;".$row[qty]."&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;".$row[unit]."<br>"; 

    if ($row[child] == 1) { 
        $i++; 
        $pid = $row[id]; 
            $level = "level".$i; 
        ${$level} = mssql_query("select * from bomrouting where parentid = '$pid'") or die("Failed");
    continue;
    } 
} 

$i--; 
if($i == 0){ 
    break; 
} 

}
while(1); 

    echo $output; 
} 

build_billofmaterials();  
?>