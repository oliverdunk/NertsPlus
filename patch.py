import argparse
from os import environ
from pathlib import Path
from run import run_game
import shutil
import subprocess
import sys
import urllib.request
from utils import run_command, run_exe, get_nerts_path, remove_existing_path
import zipfile


DE4DOT_URL = "https://github.com/ViRb3/de4dot-cex/releases/download/v4.0.0/de4dot-cex.zip"


def download_de4dot(bin_dir: Path) -> None:
    """
    Downloads de4dot to the bin dir and extracts it.
    """

    # Download de4dot to the bin dir
    urllib.request.urlretrieve(DE4DOT_URL, bin_dir / "de4dot-cex.zip")

    # Extract de4dot
    with zipfile.ZipFile(bin_dir / "de4dot-cex.zip", "r") as zip:
        zip.extractall(bin_dir)

    # Attempt to remove zip file
    (bin_dir / "de4dot-cex.zip").unlink()


def run_de4dot(bin_dir: Path, nerts_path: Path, cleaned_name: str) -> None:
    """
    Runs de4dot to deobfuscate the Nerts game executable.
    """

    run_exe(bin_dir / "de4dot-x64.exe", [str(nerts_path)])
    shutil.copyfile(nerts_path.parent / cleaned_name, bin_dir / "NertsOnline-cleaned.exe")


def build_patcher() -> None:
    """
    Builds the patcher and copies it to the Nerts game folder.
    """
    patcher_dir = Path().cwd() / "Patcher"

    subprocess.call("dotnet build", cwd=patcher_dir, shell=True)


def run_patcher(bin_dir: Path, nerts_path: Path) -> None:
    """
    Runs the patcher to patch the Nerts game executable, and then copies it to the Nerts game folder.
    """
    patcher_dir = Path().cwd() / "Patcher"

    run_exe(Path("bin") / "Debug/net452/NertsPlusPatcher.exe", cwd=patcher_dir)
    shutil.copyfile(patcher_dir / "bin/Debug/net452/NertsPlusPatcher.exe", nerts_path.parent / "NertsPlusPatcher.exe")

    # Copy patcher output back to Steam directory
    shutil.copyfile(bin_dir / "NertsOnline-patched.exe", nerts_path.parent / "NertsOnline-patched.exe")


def build_plugin(nerts_path: Path) -> None:

    plugin_dir = Path().cwd() / "Plugin"

    # Build and copy plugin
    run_command("dotnet", ["build"], cwd=plugin_dir)
    shutil.copyfile(plugin_dir / "bin/Debug/net452/NertsPlus.dll", nerts_path.parent / "NertsPlus.dll")
    shutil.copyfile(plugin_dir / "bin/Debug/net452/0Harmony.dll", nerts_path.parent / "0Harmony.dll")
    shutil.copyfile(plugin_dir / "bin/Debug/net452/Newtonsoft.Json.dll", nerts_path.parent / "Newtonsoft.Json.dll")


def write_steam_appid(nerts_path: Path) -> None:
    """
    Writes the steam_appid.txt file to the Nerts game folder.
    """

    NERTS_STEAM_ID = "1131190"

    with open(nerts_path.parent / "steam_appid.txt", "w") as file:
        file.write(NERTS_STEAM_ID)


def copy_textures(nerts_path: Path) -> None:
    """
    Copies the textures to the Nerts game folder.
    """

    shutil.copyfile("textures/logo_button.tex", nerts_path.parent / "Content/Packed/logo_button.tex")
    shutil.copyfile("textures/logo_button_hover.tex", nerts_path.parent / "Content/Packed/logo_button_hover.tex")


def main():

    # Parse arguments
    parser = argparse.ArgumentParser(description="NertsPlus patcher")
    parser.add_argument(
        "--skip-download",
        action=argparse.BooleanOptionalAction,
        default=False,
        help="Skip downloading and extracting de4dot")
    parser.add_argument(
        "-b",
        "--bin-dir",
        type=Path,
        default=Path("bin"),
        help="Directory to store de4dot and patched binaries")
    parser.add_argument(
        "-n",
        "--nerts-path",
        type=Path,
        help="Path to Nerts game executable")
    parser.add_argument(
        "-r",
        "--run",
        action=argparse.BooleanOptionalAction,
        default=False,
        help="Run the game")

    args = parser.parse_args()

    # If a nerts path is not provided, try to find it automatically
    # or ask for user input
    if args.nerts_path is None:
        nerts_path = get_nerts_path()
    else:
        nerts_path = args.nerts_path
    cleaned_name = f"{nerts_path.stem}-cleaned.exe"

    if args.run:
        run_game(nerts_path)
        sys.exit(0)

    if not (args.skip_download or environ.get("SKIP_DOWNLOAD")):
        if not remove_existing_path(args.bin_dir):
            print("Unable to clear the bin directory, aborting 🛑!")
            sys.exit(1)

        args.bin_dir.mkdir()

        # Download de4dot
        download_de4dot(args.bin_dir)

    run_de4dot(args.bin_dir, nerts_path, cleaned_name)
    build_patcher()
    run_patcher(args.bin_dir, nerts_path)
    build_plugin(nerts_path)
    copy_textures(nerts_path)
    write_steam_appid(nerts_path)

    print("🎉 All done! Now re-run with the --run flag to run the game.")


if __name__ == "__main__":
    main()
