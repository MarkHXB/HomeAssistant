from faster_whisper import WhisperModel
import sys
import os

model = None
model_size = "large-v3"

def transcribe_audio(input_wav_file, output_text_file, language, output_segments=True):
    segments, info = model.transcribe(input_wav_file, beam_size=5, language=language)

    print("Detected language '%s' with probability %f" % (info.language, info.language_probability))

    with open(output_text_file, "w", encoding="utf-8") as f:
        for segment in segments:
            if output_segments:
                f.write("[%.2fs -> %.2fs] %s\n" % (segment.start, segment.end, segment.text))
            else:
                f.write("%s\n" % segment.text)

    # Rename the processed file
    processed_file = input_wav_file + ".processed"
    os.rename(input_wav_file, processed_file)
    print(f"Renamed {input_wav_file} to {processed_file}")

def process_files(input_path, output_dir, language, output_segments=True):
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    if os.path.isfile(input_path):
        output_text_file = os.path.join(output_dir, os.path.basename(input_path) + ".txt")
        transcribe_audio(input_path, output_text_file, language, output_segments)
    elif os.path.isdir(input_path):
        for root, _, files in os.walk(input_path):
            for file in files:
                if not file.endswith("processed") and file.endswith(".wav"):
                    input_wav_file = os.path.join(root, file)
                    output_text_file = os.path.join(output_dir, file + ".txt")
                    transcribe_audio(input_wav_file, output_text_file, language, output_segments)
    else:
        print(f"Invalid input path: {input_path}")

if __name__ == "__main__":
    if len(sys.argv) != 5:
        print("Usage: python script.py <input_path> <output_dir> <output_segments> <language>")
        sys.exit(1)

    input_path = sys.argv[1]
    output_dir = sys.argv[2]
    output_segments = sys.argv[3].lower() in ['true', '1', 't', 'y', 'yes']
    language = sys.argv[4]

    print("Taken arguments:")
    print("input_path: %s\n" % input_path)
    print("output_dir: %s\n" % output_dir)
    print("output_segments: %s\n" % output_segments)
    print("language: %s\n" % language)

    # Run on GPU with FP16
    model = WhisperModel(model_size, device="cuda", compute_type="float16")

    process_files(input_path, output_dir, language, output_segments)
