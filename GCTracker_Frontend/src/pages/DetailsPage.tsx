import React, { useEffect, useMemo, useState } from 'react';
import { Card } from 'primereact/card';
import { classNames } from 'primereact/utils';    
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';


import styles from '../App.module.scss';
import { Button } from 'primereact/button';
import { ProgressSpinner } from 'primereact/progressspinner';

type Props = {
  cardId: string;
}

type CardInfo = {
  id: number;
  name: string;
  price: number;
  producentCode: string;
  image: string;
  storeName: string;
}

type MlResult = {
  predictedPrice: string;
  isPriceRising: number;
}

export default function DetailsPage(props: Props) {
  const [cardInfo, setCardInfo] = useState<CardInfo | undefined>(undefined);
  const [mlResult, setMlResult] = useState<MlResult | undefined>(undefined);


  useEffect(() => {
    fetch('https://gctrackerdeploy.azurewebsites.net/api/gpu/' + props.cardId)
    .then(res => res.json())
    .then(json => {
      setCardInfo({
        id: json['Id'],
        name: json['Name'],
        price: json['Price'],
        producentCode: json['ProducentCode'],
        image: json['Image'],
        storeName: json['StoreName']
      });
    })
    .catch(e => console.log(e));
  },[]);

  useMemo(() => {
    if (cardInfo) {
      fetch('https://gctrackerdeploy.azurewebsites.net/api/gpu/trends/' + cardInfo.producentCode)
      .then(res => res.json())
      .then(json => {
        setMlResult({
          predictedPrice: (json['PredictedPrice']).toFixed(2).toString(),
          isPriceRising: json['IsPriceRising']
        });
      })
      .catch(e => console.log(e));
    }
  }, [cardInfo]);

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
        <Card>
          <div className='grid'>
            <h1 className='col-12 text-center'>
              GCTracker
            </h1>
            {cardInfo &&
            <div className='col-12'>
              <div className='grid'>
                <h2 className='col-12 text-center border-primary border-solid border-1 mb-0'>
                  Card details: {cardInfo.name} 
                </h2>
                <div className='col-3 border-primary border-solid border-1'>
                  {cardInfo.image !== undefined && cardInfo.image !== null &&
                  <img style={{maxHeight: 200, maxWidth: 236}} src={'data:image/jpg;base64,' + cardInfo.image}/>
                   }
                </div>
                <div className='col-9 border-primary border-solid border-1'>
                  Specification: <br/>
                  Producent code: {cardInfo.producentCode}
                </div>
                { 
                  mlResult &&
                  <div className='col-6 border-primary border-solid border-1'>
                    Price is {
                      mlResult.isPriceRising === 1 ? <label className='text-red-700'>RISING</label> 
                        :  (mlResult.isPriceRising === 2 ? <label className='text-green-700'>DECREASING</label> 
                          : <label className='text-500'>STABLE</label>)
                    }
                  </div>
                }
                {
                  mlResult && 
                  <div className='col-6 border-primary border-solid border-1'>
                  Predicted future price is {
                    mlResult.isPriceRising === 1 ? <label className='text-red-700'>{mlResult.predictedPrice}zł</label> 
                      : (mlResult.isPriceRising === 2 ? <label className='text-green-700'>{mlResult.predictedPrice}zł</label> 
                        : <label className='text-500'>{mlResult.predictedPrice}zł</label>)
                  }
                </div>
                }
                <div className='col-12 border-primary border-solid border-1'>
                  <DataTable paginator rows={20} rowsPerPageOptions={[10, 20, 50]} scrollable scrollHeight='600px' value={[cardInfo]} 
                  showGridlines dataKey='id'
                  >
                      <Column field='storeName' header='Store name' style={{width: '25%'}} filter filterField="name" />
                      <Column field='price' header='Price [PLN]' style={{width: '25%'}} />
                  </DataTable>
                </div>
                <div className='col-12 text-center mt-4 mb-0'>
                  <Button
                  label='Go back'
                  onClick={() => {window.location.href = '/'}}
                  />
                </div>
              </div>
            </div>
            }
            {!cardInfo && 
            <ProgressSpinner/>
            }
          </div>
        </Card>
       </div>
     </div>
  )
}
