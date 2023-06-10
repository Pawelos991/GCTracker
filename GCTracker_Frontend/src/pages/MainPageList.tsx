import React, { useEffect, useMemo, useState } from 'react';
import { Card } from 'primereact/card';
import { classNames } from 'primereact/utils';    
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { ProgressSpinner } from 'primereact/progressspinner';


import styles from '../App.module.scss';
import { FilterMatchMode } from 'primereact/api';

type CardInfo = {
  id: number;
  name: string;
  price: number;
  producentCode: string;
  image: string;
}

const mapJsonToCards = (json: Array<any>) => {
  const cards: Array<CardInfo> = [];
  json.forEach(temp => {
    cards.push({
      id: temp.Id,
      name: temp.Name,
      price: temp.Price,
      producentCode: temp.ProducentCode,
      image: temp.ImageAddress
    });
  });
  return cards;
}

export default function MainPageList() {
  const [cards, setCards] = useState<Array<CardInfo>>();
  const [tableReady, setTableReady] = useState<boolean>(false);
  const filters = {
    name: {value: null, matchMode: FilterMatchMode.CONTAINS},
    price: {value: null, matchMode: FilterMatchMode.EQUALS},
    producentCode: {value: null, matchMode: FilterMatchMode.CONTAINS}
  }

  useEffect(() => {
    fetch('https://gctrackerdeploy.azurewebsites.net/api/gpu/all')
    .then(res => res.json())
    .then(json => {
      setCards(mapJsonToCards(json));
      setTableReady(true);
    })
    .catch(e => console.log(e))
  },[])

  return (
    <div
        className={classNames(
          'flex align-items-center justify-content-center',
          styles.pageRoot,
        )}
      >
        <div
          className={classNames(
            ' flex align-items-center justify-content-center',
            styles.cardContainer,
          )}
        >
        <Card style={{ borderRadius: '25px'}}>
          <div className='grid'>
            <h1 className='col-12 text-center m-1'>
              GCTracker
            </h1> 
            {
              tableReady &&
              <div className='col-12 m-0'>
              <DataTable className='overflow-hidden' paginator rows={20} rowsPerPageOptions={[10, 20, 50]} scrollable scrollHeight='600px' value={cards} 
              //style={{ minWidth: '1000px', maxWidth: '1000px'}} 
              showGridlines dataKey='id'
              onRowSelect = {(row) => {window.location.href = '/?id=' + (row.data.id).toString()}} selectionMode="single"
              filters={filters} filterDisplay="row"
              >
                  <Column field='id' header='ID' hidden/>
                  <Column field='name' header='Name' className='overflow-hidden' style={{width: '50%'}} filter filterField="name" />
                  <Column field='price' header='Price [PLN]' className='overflow-hidden' style={{width: '25%'}} filter />
                  <Column field='producentCode' header='Producer code' className='overflow-hidden' style={{width: '25%'}} filter />
              </DataTable>
            </div> 
            }
            {
              !tableReady &&
              <ProgressSpinner/>
            }
          </div>
        </Card>
       </div>
     </div>
  )
}
