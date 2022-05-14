import os
import subprocess
from pathlib import Path
import shutil
import shlex
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


def run_exe(path, args: list[str] = [], cwd=None):
    if system == "Darwin":
        run_command("mono64", [str(path)] + args, cwd)
    else:
        run_command(path, args, cwd)


def run_command(path, args: list[str] = [], cwd=None):
    command = str(path)

    if cwd is None:
        cwd = Path().cwd()

    command_list = [command] + args

    command_str = ' '.join(shlex.quote(arg) for arg in command_list)
    print(f"running {command_str} from {cwd}")

    subprocess.call([command_str], cwd=cwd, shell=True)
