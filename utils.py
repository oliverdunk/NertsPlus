import os
import subprocess
from pathlib import Path
import shutil
import platform
import sys

system = platform.system()


def remove_existing_path(path: Path) -> bool:
    """
    Removes a pathlib.Path if it exists.

    returns:
        - True on success,
        - False on rmtree error
    """

    try:
        if path.exists():
            shutil.rmtree(path)
    except Exception:
        return False

    return True


def get_nerts_path():
    # Try to find where Nerts is installed
    if system == "Darwin":
        nertsPath = Path(os.path.join(os.path.expanduser('~'), "Library/Application Support/Steam/steamapps/common/Nerts Online/NERTS! Online.app/Contents/MacOS/NertsOnline.exe"))
    elif system == "Windows":
        nertsPath = Path("C:/Program Files (x86)/Steam/steamapps/common/Nerts Online/NERTS! Online.exe")

    while nertsPath is None or not nertsPath.exists():
        try:
            nertsPath = Path(input("Please enter the path to NertsOnline.exe: "))
        except KeyboardInterrupt:
            sys.exit()
        except Exception as e:
            print(e)

    return nertsPath


def run_exe(path, args: list[str] = None, cwd=None):
    command = str(path)

    if args is None:
        args = []

    if cwd is None:
        cwd = Path().cwd()

    command_list = [command] + args

    print(f"running {command_list} from {cwd}")

    if system == "Darwin":
        command_list.insert(0, "mono64")

    subprocess.call(command_list, cwd=cwd, shell=True)
