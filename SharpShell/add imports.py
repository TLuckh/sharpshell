
from pathlib import Path
import os
import shutil
path = Path(os.getcwd())

for (dirpath, dirnames, filenames) in os.walk(path):
   
    if dirpath.endswith("bin") or dirpath.endswith("obj"):
        shutil.rmtree(dirpath, ignore_errors=True)
        print(dirpath)


# def line_prepender(filename, line):
#     with open(filename, 'r+',encoding="utf-8") as f:
#         try:
#             content = f.read()
#             f.seek(0, 0)
            
#         except Exception as e:
#             print(f"Error processing {filename}: {e}")
#             return

#         f.write(line.strip() + '\n' + content)

# path = Path(os.getcwd())
# print(path)
# for p in path.rglob("*.cs"):
#     line_prepender(p, "using NUnit.Framework.Legacy;")