@echo off
echo ------------ Preparing artifacts ------------
robocopy "%~dp0..\docs\Natsnudasoft\Help" "%~dp0..\src\NatsnudaLibrary\bin\Release" Natsnudasoft.NatsnudaLibrary.xml /NDL /NJH /NJS /NP /NS /NC
robocopy "%~dp0..\docs\Natsnudasoft\Help" "%~dp0..\src\TestExtensions\bin\Release" Natsnudasoft.TestExtensions.xml /NDL /NJH /NJS /NP /NS /NC
robocopy "%~dp0..\src\TestExtensions\bin\Release" artifact\TestExtensions *.dll *.xml *.pdb /XF *.dll.CodeAnalysisLog.xml /NDL /NJH /NJS /NP /NS /NC
robocopy "%~dp0..\src\NatsnudaLibrary\bin\Release" artifact\NatsnudaLibrary *.dll *.xml *.pdb /XF *.dll.CodeAnalysisLog.xml /NDL /NJH /NJS /NP /NS /NC
7z a NatsnudaLibrary_Release_Any_CPU.zip .\artifact\*
IF %errorlevel% LEQ 1 echo ------------ Artifacts prepared ------------
IF %errorlevel% LEQ 1 exit /B 0