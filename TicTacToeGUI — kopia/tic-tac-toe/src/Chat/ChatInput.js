import React, { useState } from 'react';

const ChatInput = (props) => {
    const [user, setUser] = useState('');
    const [message, setMessage] = useState('');
    const [userId, setUserId] = useState(1);

    const onSubmit = (e) => {
        e.preventDefault();

        const isUserProvided = user && user !== '';
        const isMessageProvided = message && message !== '';

        if (isUserProvided && isMessageProvided) {
            props.sendMessage(user, message);
        } 
        else {
            alert('Please insert an user and a message.');
        }
    }

    const onUserUpdate = (e) => {
        setUser(e.target.value);
    }

    const onMessageUpdate = (e) => {
        setMessage(e.target.value);
    }

    const onUserIdUpdate = (e) => {
        setUserId(e.target.value);
    }

    const searchForGame = () =>{
        props.searchForGame(userId);
    }

    const makeMove = () => {
        props.makeMove(userId,[1,1])
    }
    return (
        <>
        <form 
            onSubmit={onSubmit}>
            <label htmlFor="user">User:</label>
            <br />
            <input 
                id="user" 
                name="user" 
                value={user}
                onChange={onUserUpdate} />
            <br/>

            <input 
                id="user" 
                name="user" 
                value={userId}
                onChange={onUserIdUpdate} />
            <label htmlFor="message">Message:</label>
            <br />
            <input 
                type="text"
                id="message"
                name="message" 
                value={message}
                onChange={onMessageUpdate} />
            <br/><br/>
            <button>Submit</button>
        </form>
        <button onClick={searchForGame}>search For game</button>
        <button onClick={makeMove}>make move</button>
        </>
    )
};

export default ChatInput;