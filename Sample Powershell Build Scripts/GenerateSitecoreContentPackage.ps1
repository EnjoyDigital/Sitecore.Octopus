


task GenerateSitecorePackage {
		
	$octopus_projectName = "Trafalgar"
	$octopus_environtmentName = "Staging"

    $octopusVersionNumber =  exec { & "$tools_dir\Aqueduct.OctopusHelper\Aqueduct.OctopusDeployHelper.exe" "$octopus_projectName" "$octopus_environtmentName" }
	$octopusVersionNumber = $octopusVersionNumber.Substring(2)
	
	Write-Host ("the latest deployed release version number is : $octopusVersionNumber ")
	
	[xml]$response = $teamcity.GetBuild($octopusVersionNumber)

	new-item "$base_dir\src\data\currentserialization\"  -itemType directory -force -ErrorAction SilentlyContinue	
	
	$v = $response.builds.build.id
	
	Write-Host ("the build id is : $v")
	
	$s = "\\aque-build\e$\TeamCity\Server Configuration\system\Artifacts\Trafalgar\1. CI\$v\serialization"  
	
	Write-Host ("the seralisation source path is : $s")
	
	$t = "$base_dir\src\data\currentserialization\" 
	
	Write-Host ("the seralisation target path is : $t")
	
	Remove-Item "$t" -recurse
	Copy-Item "$s" "$t" -recurse
	
	$source = "$base_dir\src\data\currentserialization" 
	$target = "$base_dir\src\data\serialization" 	
	$output = "$frontend_base_dir\Packages\00-TemplatesAndLayouts.update" 
	$pubpath = "$frontend_base_dir\Packages\ItemsToPublish.json" 
		
	new-item "$frontend_base_dir\Packages" -itemType directory
	
	exec { & "$tools_dir\SitecoreCourier\Sitecore.Courier.Runner.exe" "/target:$target" "/source:$source" "/output:$output" "/publishingListPath:$pubpath" }
	
	#move content packages i
	$packageSource = "$base_dir\src\data\Packages" 
	$packageDest = "$frontend_base_dir" 
	Copy-Item "$packageSource" "$packageDest" -recurse -force
}
