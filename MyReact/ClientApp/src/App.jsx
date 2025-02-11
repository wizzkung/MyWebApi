import React, { useState, useEffect } from 'react';
import { Route } from 'react-router';
import Main from './components/Main';
const App = () => {
    return (
        <Layout>
            <Route exact path='/' component={Main} />
        </Layout>
    )
}

export default App