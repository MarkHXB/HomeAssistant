import speech_recognition as sr
import sys

def main(automatic_shutdown_on_silence_in_ms, output_file_path):
    # Initialize the recognizer
    r = sr.Recognizer()

    # Set the energy threshold
    r.energy_threshold = 300

    # Set the silence duration for automatic shutdown
    r.pause_threshold = automatic_shutdown_on_silence_in_ms / 1000.0

    # Start listening to the microphone
    with sr.Microphone() as source:
        print("Please speak something...")
        try:
            # Listen for the first phrase and extract it into audio data
            audio_data = r.listen(source, timeout=10)
            print("Recognizing...")

            # Convert speech to text
            text = r.recognize_google(audio_data)
            print(f"You said: {text}")

            # Save the recognized text to the specified file
            with open(output_file_path, 'w') as file:
                file.write(text)

        except sr.WaitTimeoutError:
            print("Listening timed out while waiting for phrase to start")
            sys.exit(0)
        except sr.UnknownValueError:
            print("Google Speech Recognition could not understand audio")
        except sr.RequestError as e:
            print(f"Could not request results from Google Speech Recognition service; {e}")

if __name__ == "__main__":
    if len(sys.argv) == 3:
        try:
            automatic_shutdown_on_silence_in_ms = int(sys.argv[1])
            output_file_path = sys.argv[2]
            main(automatic_shutdown_on_silence_in_ms, output_file_path)
        except ValueError:
            print("Please provide the silence duration in milliseconds as an integer.")
    else:
        print("Usage: python script.py <automatic_shutdown_on_silence_in_ms> <output_file_path>")