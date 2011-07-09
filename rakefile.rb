require 'rubygems'
require 'albacore'
require 'rake/clean'

NANCY_VERSION = "0.6.0"
OUTPUT = "build"
CONFIGURATION = 'Release'
SHARED_ASSEMBLY_INFO = 'src/SharedAssemblyInfo.cs'
SOLUTION_FILE = 'src/Nancy.Bootstrappers.Autofac.sln'
PACKAGES_FOLDER = 'src/packages'

Albacore.configure do |config|
	config.log_level = :verbose
	config.msbuild.use :net4
	config.xunit.command = "dependencies/Nancy/tools/xunit/xunit.console.clr4.x86.exe"
end

desc "Compiles solution and runs unit tests"
task :default => [:clean, :version, :compile, :test, :publish, :package]

desc "Executes all MSpec and Xunit tests"
task :test => [:xunit]

#Add the folders that should be cleaned as part of the clean task
CLEAN.include(OUTPUT)
CLEAN.include(FileList["src/**/#{CONFIGURATION}"])

desc "Update shared assemblyinfo file for the build"
assemblyinfo :version => [:clean] do |asm|
	asm.version = NANCY_VERSION
	asm.company_name = "Nancy"
	asm.product_name = "Nancy.Bootstrappers.Autofac"
	asm.title = "Nancy.Bootstrappers.Autofac"
	asm.description = "An Autofac Bootstrapper for the Nancy web framework"
	asm.copyright = "Copyright (C) Andreas Hakansson, Steven Robbins and contributors"
	asm.output_file = SHARED_ASSEMBLY_INFO
end

desc "Compile solution file"
msbuild :compile => [:version, :setup] do |msb|
	msb.properties :configuration => CONFIGURATION
	msb.targets :Clean, :Build
	msb.solution = SOLUTION_FILE
end

desc "Gathers output files and copies them to the output folder"
task :publish => [:compile] do
	Dir.mkdir(OUTPUT)
	Dir.mkdir("#{OUTPUT}/binaries")

	FileUtils.cp_r FileList["src/**/#{CONFIGURATION}/*.dll", "src/**/#{CONFIGURATION}/*.pdb"].exclude(/obj\//).exclude(/.Tests/), "#{OUTPUT}/binaries"
end

desc "Executes xUnit tests"
xunit :xunit => [:compile] do |xunit|
	tests = FileList["src/**/#{CONFIGURATION}/*.Tests.dll"].exclude(/obj\//)

	xunit.assemblies = tests
end	

desc "Zips up the built binaries for easy distribution"
zip :package => [:publish] do |zip|
	Dir.mkdir("#{OUTPUT}/packages")

	zip.directories_to_zip "#{OUTPUT}/binaries"
	zip.output_file = "NancyFx-Latest.zip"
	zip.output_path = "#{OUTPUT}/packages"
end

desc "Downloads all referenced NuGet package dependencies"
task :setup do
	FileList["**/packages.config"].each do |file|
		sh "nuget install #{file} /OutputDirectory #{PACKAGES_FOLDER}"
	end
end



#TODO:
#-----
#  6. TeamCity integration
#  7. Documentation (docu?) - Started, seems to have trouble with .NET 4 assembilies. Needs investigation.
#  8. Test coverage report (NCover?)
#  9. NuGet task (waiting for albacore pull)
# 10. Git info into shared assemby info (see fubumvc sample, also psake sample in mefcontrib)

#DONE:
#-----
#  1. Copy dlls to build folder (not tests or demo files) - DONE
#  2. Fix test tasks - DONE
#  3. Set task dependencies - DONE
#  4. Zip binaries with docs (named with version number) - DONE
#  5. Create a how to build file - DONE
