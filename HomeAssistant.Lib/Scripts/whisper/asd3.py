from faster_whisper import WhisperModel
import sys

def transcribe_audio(input_wav_file, output_text_file, output_segments = True):
    model_size = "large-v3"

    # Run on GPU with FP16
    model = WhisperModel(model_size, device="cuda", compute_type="float16")

    # or run on GPU with INT8
    # model = WhisperModel(model_size, device="cuda", compute_type="int8_float16")
    # or run on CPU with INT8
    # model = WhisperModel(model_size, device="cpu", compute_type="int8")

    segments, info = model.transcribe(input_wav_file, beam_size=5)

    print("Detected language '%s' with probability %f" % (info.language, info.language_probability))

    with open(output_text_file, "w", encoding="utf-8") as f:
        for segment in segments:
            if output_segments == True:
                f.write("[%.2fs -> %.2fs] %s\n" % (segment.start, segment.end, segment.text))
            else:
                f.write("%s\n" % segment.text)

if __name__ == "__main__":
    if len(sys.argv) != 4:
        print("Usage: python script.py <input_wav_file> <output_text_file> <output_segments>")
        sys.exit(1)

    input_wav_file = sys.argv[1]
    output_text_file = sys.argv[2]
    output_segments = sys.argv[3]

    print("Taken arguements:")
    print("input_wav_file: %s\n" % input_wav_file)
    print("output_text_file: %s\n" % output_text_file)
    print("output_segments: %s\n" % output_segments)

    transcribe_audio(input_wav_file, output_text_file, output_segments)
