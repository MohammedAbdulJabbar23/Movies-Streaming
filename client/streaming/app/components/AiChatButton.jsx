// components/AiChatButton.js

import Link from "next/link";

const AiChatButton = () => {
  return (
    <Link href="/ai-chat">
      <span
        className="fixed bottom-6 right-6 z-50 bg-gray-800 text-white p-4 rounded-full shadow-lg hover:bg-gray-600 transition-transform transform  flex items-center justify-center"
        aria-label="Chat with AI"
      >
        <i className="fas fa-comment-dots text-lg"></i>
      </span>
    </Link>
  );
};

export default AiChatButton;
