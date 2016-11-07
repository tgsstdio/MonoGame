function Replace-Entrypoint{
param(
	[string] $src,
	[string] $dst,
	[string] $entrypoint
	)
	
	(Get-Content $src).replace('"main"', $entrypoint) | Set-Content $dst
}
export-modulemember -function Replace-Entrypoint

// (Get-Content $src).replace('"main"', $entrypoint) | Set-Content $dst