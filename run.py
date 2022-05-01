import os
import platform
import utils

system = platform.system()

if system == "Darwin":
  os.chdir(os.path.dirname(utils.get_nerts_path()) + "/osx")
  utils.run_exe("../NertsOnline-patched.exe")
else:
  os.chdir(os.path.dirname(utils.get_nerts_path()))
  utils.run_exe("NertsOnline-patched.exe")
