using System;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using vsix.helpers;
using vsixsample.IntercomHost;
using Task = System.Threading.Tasks.Task;
using VSLangProj;

namespace vsixsample
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]

    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]

    [Guid(vsixsamplePackage.PackageGuidString)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public sealed class vsixsamplePackage : AsyncPackage, IService
    {
        /// <summary>
        /// vsixsamplePackage GUID string.
        /// </summary>
        public const string PackageGuidString = "bf6cb10b-4a79-45a5-965d-cd79eda57f90";
        public bool HostedByWcf = false;

        public Project vproject;

        public static DTE2 _dte;
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            var serviceHost = new ServiceHost(typeof(vsixsamplePackage), new Uri[] { new Uri("net.pipe://localhost/vsix") });
            serviceHost.AddServiceEndpoint(typeof(IService), new NetNamedPipeBinding(), "api");
            serviceHost.Open();
            HostedByWcf = true;

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            _dte = GetService(typeof(DTE)) as DTE2;

        }
        public vsixsamplePackage()
        {

        }

        async void IService.HelloWorld()
        {

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(); //воротаемся в UI поток.


            vproject = ProjectHelpers.GetActiveProject();
            string rootfolder = vproject.GetRootFolder(); //корневая папка проекта с csproj открытым в студии
            string vsixdllpath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "");
            string vsixfolder = Path.GetDirectoryName(vsixdllpath);

            string filename = "tmp1.cs";
            string path2addfile = Path.Combine(vsixfolder, "Templates", filename);
            var filetoadd = new FileInfo(path2addfile);

            ProjectItem projectItem = null;
            string fileprojpath = Path.Combine(rootfolder, filename);

            var filedest = new FileInfo(Path.Combine(fileprojpath));

            if (1 == 1)
            {
                if (1 == 1) //добавить файл в проект из VSIX папки
                {
                    ProjectHelpers.CopyTmpToProjectFile(vproject, filetoadd.FullName, filedest.FullName);
                }
                projectItem = vproject.AddFileToProject(filedest);

                vproject.DTE.ItemOperations.OpenFile(fileprojpath); //открыть редактор с новым файлом

                if (true) //добавить текст
                {
                    Microsoft.VisualStudio.Text.Editor.IWpfTextView view = ProjectHelpers.GetCurentTextView();

                    if (view != null)
                    {
                        try
                        {
                            ITextEdit edit = view.TextBuffer.CreateEdit();
                            edit.Insert(0, Environment.NewLine);
                            edit.Apply();
                        }
                        catch (Exception e)
                        {

                        }
                    }


                }
                if (true) //добавление абсолютной ссылки в проект
                {
                    VSProject refproject;
                    refproject = (VSProject)vproject.Object;
                    try
                    {
                        refproject.References.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\Microsoft.Build.dll");
                    }
                    catch (Exception ddd)
                    {


                    }

                }



            }
        }


        #endregion
    }
}
