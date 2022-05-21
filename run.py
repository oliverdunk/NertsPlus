from pathlib import Path
import platform
from utils import run_exe, get_nerts_path

system = platform.system()


def run_game(nerts_path: Path):
    if system == "Darwin":
        run_exe("../NertsOnline-patched.exe", cwd=(nerts_path.parent / "osx"))
    else:
        run_exe("NertsOnline-patched.exe", cwd=(nerts_path.parent))


if __name__ == "__main__":
    nerts_path = get_nerts_path()
    run_game(nerts_path)
