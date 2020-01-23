using System;
using System.Diagnostics;

namespace SuperLauncher
{
    internal class ApplicationProcess
    {
        private Process _applicationProcess;
        private Guid _applicationGuid;

        private Action<Guid, (DateTime processStart, DateTime processEnd)> _processCallback;
        private bool _exited;

        public ApplicationProcess(Guid applicationGuid, string executablePath, 
            Action<Guid, (DateTime processStart, DateTime processEnd)> processCallback)
        {
            _processCallback = processCallback;
            _applicationGuid = applicationGuid;

            _applicationProcess = new Process();
            _applicationProcess.StartInfo.FileName = executablePath;
            _applicationProcess.StartInfo.UseShellExecute = true;
            _applicationProcess.EnableRaisingEvents = true;

            _applicationProcess.ErrorDataReceived += _applicationProcess_ErrorDataReceived;
            _applicationProcess.Exited += _applicationProcess_Exited;
            _applicationProcess.Start();
        }

        ~ApplicationProcess()
        {
            if (_exited)
                return;

            if (_applicationProcess != null && !_applicationProcess.HasExited)
                _applicationProcess?.Kill();

            _applicationProcess?.Dispose();
        }

        private void _applicationProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            _exited = true;
            _applicationProcess.Dispose();
        }

        public void Stop()
        {
            if (_exited || _applicationProcess.HasExited)
                return;

            _applicationProcess.Kill();
        }

        private void _applicationProcess_Exited(object sender, EventArgs e)
        {
            if (_exited)
                return;

            _processCallback?.Invoke(_applicationGuid, (_applicationProcess.StartTime, _applicationProcess.ExitTime));
            _applicationProcess.Dispose();

            _exited = true;
        }
    }
}
