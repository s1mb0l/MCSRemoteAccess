import sys
import os

def main():
    if len(sys.argv) < 2:
        print("Error: Log file path not provided.")
        sys.exit(1)

    log_path = sys.argv[1]

    if not os.path.exists(log_path):
        print(f"Error: File at '{log_path}' does not exist.")
        sys.exit(1)

    with open(log_path, 'r', encoding='utf-8', errors='ignore') as file:
        lines = file.readlines()
        error_count = sum(1 for line in lines if "ERROR" in line or "WARN" in line)

    print(f"Analysis completed. Found {error_count} warnings/errors.")

if __name__ == "__main__":
    main()