// components/AiChatButton.js

import Link from "next/link";

const AiChatButton = () => {
  return (
    <Link href="/ai-chat">
      <span
        className="fixed bottom-6 right-6 z-50 bg-gray-800 text-white text-sm p-2 py-3 rounded-full shadow-lg hover:bg-gray-600 transition-transform transform  flex items-center justify-center"
        aria-label="Chat with AI"
      >
        <span className="mr-2">Chat With AI</span>
        <i className="fas fa-comment-dots text-lg"></i>
      </span>
    </Link>
  );
};

export default AiChatButton;
