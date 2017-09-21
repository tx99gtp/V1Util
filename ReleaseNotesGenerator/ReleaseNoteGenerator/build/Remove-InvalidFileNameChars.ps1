<#
.SYNOPSIS
Removes characters from a string that are not valid in Windows file names.

.DESCRIPTION
Remove-InvalidFileNameChars accepts a string and removes characters that are invalid in Windows file names.  It then outputs the cleaned string.  It accepts value from the pipeline by the property Name.  By default the space character is ignored, but can be included using the IncludeSpace parameter.

.PARAMETER Name
Specifies the file name to strip of invalid characters.

.PARAMETER IncludeSpace
The IncludeSpace parameter will include the space character (U+0032) in the removal process.

.INPUTS
System.String
Accepts the property Name from the pipeline

.OUTPUTS
System.String

.EXAMPLE
PS C:\> Remove-InvalidFileNameChars -Name "<This /name \is* an :illegal ?filename>.txt"
PS C:\> This name is an illegal filename.txt

This command will strip the invalid characters from the string and output a clean string.

.EXAMPLE
PS C:\> Remove-InvalidFileNameChars -Name "<This /name \is* an :illegal ?filename>.txt" -IncludeSpace
PS C:\> Thisnameisanillegalfilename.txt

This command will strip the invalid characters from the string and output a clean string, removing the space character (U+0032) as well.

.NOTES
Author:  Chris Carter
Version: 1.1
Last Updated: August 28, 2014

.Link
System.RegEx
about_Join
about_Operators
#>

#Requires -Version 2.0
[CmdletBinding()]

Param(
    [Parameter(
        Mandatory=$true,
        Position=0, 
        ValueFromPipelineByPropertyName=$true
    )]
    [String]$Name,
    [switch]$IncludeSpace
)

if ($IncludeSpace) {
    [RegEx]::Replace($Name, "[{0}]" -f ([RegEx]::Escape([String][System.IO.Path]::GetInvalidFileNameChars())), '')
}
else {
    [RegEx]::Replace($Name, "[{0}]" -f ([RegEx]::Escape(-join [System.IO.Path]::GetInvalidFileNameChars())), '')
}
