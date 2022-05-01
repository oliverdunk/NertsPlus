import os
from pathlib import Path
import platform

system = platform.system()

def get_nerts_path():
  # Try to find where Nerts is installed
  if system == "Darwin":
    nertsPath = Path(os.path.join(os.path.expanduser('~'), "Library/Application Support/Steam/steamapps/common/Nerts Online/NERTS! Online.app/Contents/MacOS/NertsOnline.exe"))
  elif system == "Windows":
    nertsPath = Path("C:/Program Files (x86)/Steam/steamapps/common/Nerts Online/NertsOnline.exe")

  while nertsPath is None or not nertsPath.exists():
    try:
      nertsPath = Path(input("Please enter the path to NertsOnline.exe: "))
    except KeyboardInterrupt:
      quit()
    except:
      print("")
      pass

  return nertsPath

def run_exe(path, args = None):
  command = str(path)

  if not args is None:
    command = command + " " + args

  if system == "Darwin":
    os.system("mono64 " + command)
  else:
    os.system(command)
