```markdown
# IGCLWrapper

## Overview

This repository provides a wrapper for IGCL (Intel Graphics Control Library), enabling developers to interact with Intel GPU features programmatically. It simplifies the integration of IGCL functionalities into your applications.

## Features

- Access and control Intel GPU settings.
- Simplified API for IGCL integration.
- Customizable and extensible for various use cases.

## Getting Started

### Prerequisites

- Intel GPU with IGCL support.
- Visual Studio 2022 or later installed on your system (we use msbuild).

### Build Instructions

1. Open a terminal and navigate to the root directory of the repository.
2. Run the `prepare_igcl.ps1` script to download the IGCL SDK and install SWIG:
    ```powershell
    powershell.exe -ExecutionPolicy Bypass -File prepare_adlx.ps1
    ```
    This script will:
    - Install SWIG if not already installed.
    - Download the IGCL SDK.

3. After the script completes, run the `rebuild_igcl.bat` file to compile the DLLs and generate the binding files:
    ```cmd
    rebuild_igcl.bat
    ```

4. Once the build process is complete, the generated files will be available in the output directory.
    - The 64-bit DLL (IGCLWrapper.dll) will be available in IGCLWrapper/x64/Debug
    - Tbe C# Bindings files will be available in IGCLWrapper/cs_bindings/*.cs

### How to Use

1. Copy the IGCLWrapper.dll file into the root folder of your C# project.
2. Right click on the IGCLWrapper.dll file and set it to 'Copy if Newer' so that the file is included when your C# project is built.
3. Copy the IGCLWrapper/cs_bindings folder into your project.
4. Add `ūse IGCLWrapper;` into any source file that you want to use the IGCLWrapper within.

### Test Instructions
IMPORTANT: The Unit Tests will only work if run on a computer with Intel GPU hardware in it.

1. Run the `test_igcl.bat` file to compile IGCLWrapper.Tests project to test IGCLWrapper.dll can be loaded and will work:
    ```cmd
    test_igcl.bat
    ```

```