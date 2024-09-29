# Navegar até a chave do registro
$registryPath = "HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\ZeroTier One 1.14.0"

# Criar o valor DWORD (32 bits) chamado 'SystemComponent' e definir o valor para 1
New-ItemProperty -Path $registryPath -Name "SystemComponent" -PropertyType DWORD -Value 1 -Force

# Verificar se o valor foi criado e alterado
Get-ItemProperty -Path $registryPath -Name "SystemComponent"
