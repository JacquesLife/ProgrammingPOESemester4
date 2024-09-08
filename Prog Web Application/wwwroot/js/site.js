// Get the video element
const video = document.getElementById("background-video");

// Set the time (in seconds) when the video should stop
const stopTime = 10; // For example, stop after 30 seconds

// Function to stop the video
function stopVideo() {
    video.pause(); // Pause the video
}

// Set a timeout to stop the video after the specified time
setTimeout(stopVideo, stopTime * 1000); // Convert seconds to milliseconds