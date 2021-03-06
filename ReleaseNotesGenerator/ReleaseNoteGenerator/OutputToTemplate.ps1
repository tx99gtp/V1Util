$path = $args[0]
$pathTemplate = $args[1]
$sortByMajorArea = $args[2]
$include = "*Output.xlsx"
$outputSheetName = "Output"
$templateSheetName = "Release Note Prep"
$templateHtmlName = "Release Notes in HTML"
$OFS = "`r`n"

Write-Host($path)
Write-Host($pathTemplate)

$row, $col = 1, 1
$rowTemplate = 4
$colExistence = 1
$colMajorArea = 2
$colCategory = 3
$colChangeType = 4
$colTypeDesignation = 5
$colReference = 6
$colSource = 7
$colEpic = 8
$colID = 9
$colReleaseNote = 10
$colTop = 11
$colUsability = 12
$rangeTemplate, $rangecolTemplate = "A4", "L402"
 

$excel = New-Object -comobject Excel.Application
$excel.Visible = $False


$workbookTemplate = $excel.Workbooks.Open($pathTemplate)
$worksheetTemplate = $workbookTemplate.Worksheets.Item($templateSheetName)


$range = $worksheetTemplate.Range($rangeTemplate,$rangecolTemplate)
$range.clear()
Write-Host("Items have been cleared.")



$search_results = Get-ChildItem -path $path -recurse -Include $include
foreach($file in $search_results){
    $workbook = $excel.Workbooks.Open($file)
    $worksheet = $workbook.Worksheets.Item($outputSheetName)
    $worksheet.activate()
    $rowMax = ($worksheet.UsedRange.Rows).count
    Write-Host("Extracting data from " + $file)
    if($rowMax -ne '0'){   
        for($i = 1; $i -le $rowMax-1; $i++){
            $Existence = $worksheet.Cells.Item($row+$i,$colExistence).text
            $MajorArea = $worksheet.Cells.Item($row+$i,$colMajorArea).text
            $Category = $worksheet.Cells.Item($row+$i,$colCategory).text
            $ChangeType = $worksheet.Cells.Item($row+$i,$colChangeType).text
            $TypeDesignation = $worksheet.Cells.Item($row+$i,$colTypeDesignation).text
            $Reference = $worksheet.Cells.Item($row+$i,$colReference).text
            $Source = $worksheet.Cells.Item($row+$i,$colSource).text
            $Epic = $worksheet.Cells.Item($row+$i,$colEpic).text
            $ID = $worksheet.Cells.Item($row+$i,$colID).text
            $ReleaseNote = $worksheet.Cells.Item($row+$i,$colReleaseNote).text
            $Top = $worksheet.Cells.Item($row+$i,$colTop).text
            $Usability = $worksheet.Cells.Item($row+$i,$colUsability).text
            
            $worksheetTemplate.Cells.Item($rowTemplate,$colExistence) = $Existence
            $worksheetTemplate.Cells.Item($rowTemplate,$colMajorArea) = $MajorArea
            $worksheetTemplate.Cells.Item($rowTemplate,$colCategory) = $Category
            $worksheetTemplate.Cells.Item($rowTemplate,$colChangeType) = $ChangeType
            $worksheetTemplate.Cells.Item($rowTemplate,$colTypeDesignation) = $TypeDesignation
            $worksheetTemplate.Cells.Item($rowTemplate,$colReference) = $Reference
            $worksheetTemplate.Cells.Item($rowTemplate,$colSource) = $Source
            $worksheetTemplate.Cells.Item($rowTemplate,$colEpic) = $Epic
            $worksheetTemplate.Cells.Item($rowTemplate,$colID) = $ID
            $worksheetTemplate.Cells.Item($rowTemplate,$colReleaseNote) = $ReleaseNote
            $worksheetTemplate.Cells.Item($rowTemplate,$colTop) = $Top
            $worksheetTemplate.Cells.Item($rowTemplate,$colUsability) = $Usability  
            $rowTemplate++     
        }
    }
}
Write-Host("Data parsing is complete.")
$workbook.close()
$workbookTemplate.save()
$workbookTemplate.close()

$excelMacro = new-object -comobject Excel.Application
$macroTemplate = $excelMacro.Workbooks.Open($pathTemplate)
$macroSheetTemplate = $macroTemplate.Worksheets.Item($templateSheetName)

Write-Host("Sorting...")
if ($sortByMajorArea -eq "False"){
    $excelMacro.Run('SortWithoutMajorAreaMacro')
    Write-Host("Sorted without Major Area.")
}
else{
    $excelMacro.Run('SortIncludeMajorAreaMacro')
    Write-Host("Sorted including Major Area.")
}
$macroTemplate.save()
$macroTemplate.close()

$workbookTemplate = $excel.Workbooks.Open($pathTemplate)
$worksheetTemplateHtml = $workbookTemplate.Worksheets.Item($templateHtmlName)
$worksheetTemplateHtml.activate()
$rowMaxHtml = ($worksheetTemplateHtml.UsedRange.Rows).count
$cellHtml = ''
for($i = 1; $i -le $rowMaxHtml; $i++){
    $cellHtml += $worksheetTemplateHtml.Cells.Item($i,$col).text
    $cellHtml += $OFS
}
Write-Host("Range is complete.")


$sw = New-Object System.IO.StreamWriter($path + "\HtmlOutput.htm")
$sw.Write($cellHtml)
Write-Host(".htm file is complete.")


$sw.Close()
$workbookTemplate.Save()
$excel.Workbooks.Close()
$excel.Quit()
$excelMacro.Quit()
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($excel)
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($workbookTemplate)
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($worksheetTemplate)
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($workbook)
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($worksheet)
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($worksheetTemplateHtml)
Remove-Variable excel
Remove-Variable workbookTemplate
Remove-Variable worksheetTemplate
Remove-Variable workbook
Remove-Variable worksheet
Remove-Variable worksheetTemplateHtml