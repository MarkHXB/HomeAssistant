import pytesseract
from PIL import Image
import os
import sys

def image_to_text(image_path, tesseract_path=None):
    if tesseract_path:
        pytesseract.pytesseract.tesseract_cmd = tesseract_path

    try:
        text = pytesseract.image_to_string(Image.open(image_path))
        return text
    except FileNotFoundError:
        print(f"Error: Image file '{image_path}' not found.")
        return None
    except Exception as e:
        print(f"Error occurred: {e}")
        return None

def save_text_to_file(text, file_path):
    output_dir = os.path.dirname(file_path)
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    try:
        with open(file_path, 'w') as file:
            file.write(text)
        print(f"Text saved to '{file_path}'.")
    except Exception as e:
        print(f"Error occurred while saving text: {e}")

if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python script.py <image_path> <output_text_file>")
        sys.exit(1)

    image_path = sys.argv[1]
    output_text_file = sys.argv[2]
    tesseract_path = "C:/Program Files (x86)/Tesseract-OCR/tesseract.exe"  # Adjust as needed

    text = image_to_text(image_path, tesseract_path)
    if text:
        save_text_to_file(text, output_text_file)
