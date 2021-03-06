Set-ItemProperty "HKCU:\Software\Microsoft\Windows\Windows Error Reporting" -Name DontShowUI -Value 1

$(If ($args.count -eq 0) { Get-ChildItem ".\programs" } Else { Get-Item (".\programs\" + $args[0]) }) |
ForEach-Object {
	Write-Host -NoNewline $_.Name.PadRight(32, " ")

	..\Compiler\bin\Debug\netcoreapp3.1\Compiler.exe $_.FullName > $null 2> $null
	if ($LASTEXITCODE -ne 0) {
		Write-Output "Compile error"
		return
	}

	$m = Measure-Command { .\Program.exe > .\output.txt 2> $null }
	if ($LASTEXITCODE -ne 0) {
		Write-Output "Runtime error"
		return
	}

	$c1 = Get-Content $_.FullName.Replace(".lisp", ".txt").Replace("programs", "answers")
	$c2 = Get-Content output.txt
	if ($c1 -eq $null -and $c2 -eq $null) {
		Write-Output ("OK  " + $m.TotalMilliseconds)
		return
	}
	if ($c1 -eq $null -or $c2 -eq $null) {
		Write-Output "Diff"
		return
	}
	if (Compare-Object $c1 $c2) {
		Write-Output "Diff"
	}
	else {
		Write-Output ("OK  " + $m.TotalMilliseconds)
	}
}

Set-ItemProperty "HKCU:\Software\Microsoft\Windows\Windows Error Reporting" -Name DontShowUI -Value 0
