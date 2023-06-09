import React from 'react';

import MainPageList from './MainPageList';
import DetailsPage from './DetailsPage';

export default function MainPage() {
  const queryString = window.location.search;
  const urlParams = new URLSearchParams(queryString);
  const cardId = urlParams.get('id');
  let mainPage = false;
  if (cardId === null) {
    return (
      <MainPageList/>
    )
  } else {
    return (
      <DetailsPage cardId={cardId}/>
    )
  }
}
